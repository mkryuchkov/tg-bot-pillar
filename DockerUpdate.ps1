docker build ./ -t ghcr.io/mkryuchkov/tg-bot-pillar:latest
docker push ghcr.io/mkryuchkov/tg-bot-pillar:latest
docker image prune -f