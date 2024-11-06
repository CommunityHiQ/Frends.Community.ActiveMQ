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
| Queue            | string | Queue from which messages will be consumed   | testqueue									|

### Options

| Property          | Type | Description                                       | Example |
|-------------------|------|---------------------------------------------------|---------|
| MessageReceiveTimeout | int | Specifies the maximum duration, in seconds, to wait for receiving a message from the queue. | 10 |
| MaxMessagesToConsume | int | Maximum number of messages to receive. A value of 0 means no limit. | 5 |
| ThrowErrorIfEmpty | bool | Should the task throw an error if no messages are consumed? | true |
| TaskExecutionTimeout | int | Specifies the maximum time, in milliseconds, to wait for the task to complete before considering it timed out. | 5000 |

### Result

| Property | Type | Description | Example | | ---------|--------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| Messages | Message[] { string Type, dynamic Content } | Messages consumed from the queue. Content's type depends on message type, e.g. for text messages it is `string` and for byte messages it is `byte[]`. | [ { Type = "Text", Content = "my message" }, { Type = "Bytes", Content = [ 1, 2, 3, 4, 5 ] } ] |

## Produce

### Task Parameters

### Input

| Property         | Type   | Description | Example |
|------------------|--------|----------------------------------------------|--------------------------------------------|
| ConnectionString | secret | Connection string to ActiveMQ. | activemq:tcp://admin:admin@localhost:61616 |
| Queue            | string | Queue to which the message will be sent. | myQueue |
| Message          | string | Message which will be sent to the queue. | This is a test message. |

### Result

| Property | Type     | Description                    | Example |
| ---------|----------|--------------------------------|---------|
| Success  | bool  | Indicates whether the message was sent successfully. | true |

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

| Version | Changes                                                                                                                                    |
|---------|--------------------------------------------------------------------------------------------------------------------------------------------|
| 1.0.0   | Initial implementation of Consume-Task.                                                                                                    |
| 1.0.1   | Consume: Added Apache.NMS version 2.0.0 as dependency.                                                                                     |
| 2.0.0   | Initial implementation of Produce-Task, Documentation fix for Consume-Task.                                                                |
| 3.0.0   | Consume: result's Messages property changed from array of strings to Message { string Type, dynamic Message }; added byte message support. |
| 3.1.0   | Consume: Added MaxMessagesToConsume parameter to Options                                                                                   |
| 3.2.0   | Consume: Added Timeout parameter for single message and added cancellation tokens to Task.Run and Task.Wait methods.                       |
| 4.0.0   | Consume: Added Options.TaskExecutionTimeout parameter and renamed Options.Timeout to Options.MessageReceiveTimeout.                       |