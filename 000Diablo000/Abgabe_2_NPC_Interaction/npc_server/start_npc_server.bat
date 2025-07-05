@echo off
echo Starte NPC-Server...

REM Docker Container neu bauen und starten
docker build -t npc-server .
docker stop npc-server
docker rm npc-server

docker run -d -p 8000:8000 --name npc-server npc-server

echo NPC-Server läuft unter: http://localhost:8000
pause