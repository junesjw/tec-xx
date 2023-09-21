# Discord Bot

## Overview

This is a Discord bot built using C# and deployed on a Linux server using Docker. The bot is capable of various functionalities, which can be extended as per requirements.

## Prerequisites

- .NET SDK
- Docker
- Docker Compose

## Project Structure

- `App.config`: Contains application configuration settings.
- `Bot.cs`: Contains the main logic for the bot.
- `ConfigJson.cs`: A C# class for handling JSON configuration.
- `Dockerfile`: Dockerfile for building the Docker image.
- `Program.cs`: Entry point for the application.
- `config.json`: JSON-formatted configuration settings.
- `docker-compose.yml`: Docker Compose file for running the bot.
- `tec-xx.csproj`: C# project file.
- `tec-xx.db`: SQLite Database file.
- `tec-xx.sln`: C# solution file.

## Setup

1. **Clone the Repository**

    ```bash
    git clone https://github.com/your-username/discord-bot.git
    cd discord-bot
    ```

2. **Build the Docker Image**

    ```bash
    docker build -t discord-bot .
    ```

3. **Run the Docker Compose**

    ```bash
    docker-compose up -d
    ```

4. **Check if the Bot is Running**

    Open Discord and check if the bot is online.

## Configuration

You can configure the bot by modifying the `config.json` file.

## Database

The bot uses a database (SQLite) for storing data. The database file is `tec-xx.db`.

## Contributions

Feel free to contribute to this project by submitting pull requests or issues.

## License

This project is licensed under the MIT License.

