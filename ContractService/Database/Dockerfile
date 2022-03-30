# docker build -t db .
# docker run -p 3306:3306 db

FROM --platform=linux/amd64 mysql:latest
ENV MYSQL_ALLOW_EMPTY_PASSWORD yes
COPY /init.sql /docker-entrypoint-initdb.d/.
