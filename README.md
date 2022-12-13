# Frends.Community.ActiveMQ
Frends Tasks for ActiveMQ.

[![Actions Status](https://github.com/CommunityHiQ/Frends.Community.ActiveMQ/workflows/PackAndPushAfterMerge/badge.svg)](https://github.com/CommunityHiQ/Frends.Community.ActiveMQ/actions)
![MyGet](https://img.shields.io/myget/frends-community/v/Frends.Community.ActiveMQ)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

- [Installing](#installing)
- [Tasks](#tasks)
  - [Consume](#consume)
  - [Produce](#produce)
- [Building](#building)
- [License](#license)
- [Contributing](#contributing)
- [Change Log](#change-log)

# Installing
You can install the Task via Frends UI Task View or you can find the NuGet package from the following NuGet feed
https://www.myget.org/F/frends-community/api/v3/index.json and in Gallery view in MyGet https://www.myget.org/feed/frends-community/package/nuget/Frends.Community.ActiveMQ

# Tasks

## Consume

### Task Parameters

### Input

| Property         | Type   | Description                                  | Example                                    |
|------------------|--------|----------------------------------------------|--------------------------------------------|
| ConnectionString | secret | Connection string to ActiveMQ.               | activemq:tcp://admin:admin@localhost:61616 |
| Queue            | string | Queue name from which messages are consumed. | testqueue                                  |

### Options

| Property          | Type | Description                                       | Example |
|-------------------|------|---------------------------------------------------|---------|
| ThrowErrorIfEmpty | bool | Should the task fail if no messages are consumed? | true    |

### Result

| Property | Type     | Description                   | Example                             |
| ---------|----------|-------------------------------|-------------------------------------|
| Messages | string[] | Messages consumed from queue. | ["first message", "second message"] |

## Produce

### Task Parameters

### Input

| Property         | Type   | Description                                  | Example                                    |
|------------------|--------|----------------------------------------------|--------------------------------------------|
| ConnectionString | secret | Connection string to ActiveMQ.               | activemq:tcp://admin:admin@localhost:61616 |
| Queue            | string | Queue name from which messages are consumed. | testqueue                                  |
| Message          | string | Message which will be sent to queue.         | This is a test message.                    |

### Result

| Property | Type     | Description                    | Example |
| ---------|----------|--------------------------------|---------|
| Success  | boolean  | Message was sent successfully? | true    |

# Building

Clone a copy of the repo.

`git clone https://github.com/CommunityHiQ/Frends.Community.ActiveMQ.git`

Build the project.

`dotnet build`

Run Tests.<br/>
Test require test ActiveMQ, which you can start using Docker.

`cd Frends.Community.ActiveMQ.Tests && docker-compose up -d`

You can execute test with the following command.

`dotnet test`

Create a NuGet package.

`dotnet pack --configuration Release`

# License

This project is licensed under the MIT License - see the LICENSE file for details.

# Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

# Change Log

| Version | Changes                                                                     |
|---------|-----------------------------------------------------------------------------|
| 1.0.0   | Initial implementation of Consume-Task.                                     |
| 1.0.1   | Consume: Added Apache.NMS version 2.0.0 as dependency.                      |
| 2.0.0   | Initial implementation of Produce-Task, Documentation fix for Consume-Task. |
