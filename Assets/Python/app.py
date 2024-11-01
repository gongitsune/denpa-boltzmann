import cv2
import mediapipe as mp
import socket
import json
import pyvirtualcam

address = ("127.0.0.1", 5000)
udp = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# MediaPipe Handsの初期化
mp_hands = mp.solutions.hands
hands = mp_hands.Hands()

# カメラを開く
camera = cv2.VideoCapture(0)


def generate_frames(virtual_cam: pyvirtualcam.Camera):
    print("Start generating frames")
    while True:
        # カメラからフレームを読み込む
        success, new_frame = camera.read()
        if not success:
            continue

        # 手の位置を認識
        image_rgb = cv2.cvtColor(new_frame, cv2.COLOR_BGR2RGB)
        results = hands.process(image_rgb)

        hand_pos_arr = []
        if results.multi_hand_landmarks:
            for hand_landmarks in results.multi_hand_landmarks:
                # 手のランドマークを描画
                mp.solutions.drawing_utils.draw_landmarks(
                    new_frame, hand_landmarks, mp_hands.HAND_CONNECTIONS
                )

                # 手の中心位置を計算
                x_list = [lm.x for lm in hand_landmarks.landmark]
                y_list = [lm.y for lm in hand_landmarks.landmark]
                center_x = sum(x_list) / len(x_list)
                center_y = sum(y_list) / len(y_list)

                height, width, _ = new_frame.shape
                screen_x = center_x * width
                screen_y = center_y * height

                hand_pos_arr.append({"x": 1 - center_x, "y": 1 - center_y})

                # 手の中心を短径で囲む矩形を描画
                radius = 50  # 短径のサイズ
                cv2.circle(
                    new_frame,
                    (int(screen_x), int(screen_y)),
                    radius,
                    (0, 255, 0),
                    2,
                )

        # 手の位置を送信
        udp.sendto(json.dumps({"positions": hand_pos_arr}).encode(), address)

        # 画像を表示
        fliped_frame = cv2.cvtColor(cv2.flip(new_frame, 1), cv2.COLOR_BGR2RGB)
        virtual_cam.send(fliped_frame)
        # cv2.imshow("Frame", fliped_frame)
        # if cv2.waitKey(1) & 0xFF == ord("q"):
        #     break
        virtual_cam.sleep_until_next_frame()


if __name__ == "__main__":
    try:
        with pyvirtualcam.Camera(width=640, height=480, fps=30) as virtual_cam:
            print("Virtual camera device: " + virtual_cam.device)
            generate_frames(virtual_cam)
    finally:
        udp.close()
        camera.release()
        cv2.destroyAllWindows()
