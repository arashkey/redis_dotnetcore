# Redis

for run project, use run.bat

## Redis server configuration

For config redis server run this command on server (centos os)

```bash
#!/bin/bash

yum install docker -y
systemctl start docker
systemctl enable docker

vim  /etc/sysctl.conf 
#Add this line to the file
vm.overcommit_memory = 1
net.core.somaxconn=65535
#then reboot system


docker pull redis
docker network create -d bridge redisnet
docker run -p 9191:6379  --restart unless-stopped -d --name RedisCache redis --requirepass 123456
```
## Project configuration
Don't forget to replace all of ip of server in the project with your server ip (127.0.0.1)
