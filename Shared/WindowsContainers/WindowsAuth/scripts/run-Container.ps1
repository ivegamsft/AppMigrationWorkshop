docker run -it -h host --security-opt "credentialspec=file://win.json" iis-site cmd
docker run -d -p 80:80 -h host --security-opt "credentialspec=file://win.json" iis-site -name aspnet
docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" aspnet