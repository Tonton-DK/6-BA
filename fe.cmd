docker container rm ba-frontend-1 -f
docker image rm ba_frontend -f
docker compose build
docker compose up