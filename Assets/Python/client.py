import socket

address = ("127.0.0.1", 5000)

# ソケットを作成
udp = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
udp.bind(address)

try:
    while True:
        # パケットを受信
        data, _ = udp.recvfrom(1024)
        print(data.decode())
finally:
    # ソケットを閉じる
    udp.close()
