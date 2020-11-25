FROM alpine:latest
RUN mkdir /code
WORKDIR /code
ADD https://dot.net/v1/dotnet-install.sh /code/dotnet-install


