# EventFlow

**An Event‑Driven Architecture Simulation with an In‑Memory Event Broker
in .NET**

------------------------------------------------------------------------

## 📖 Overview

EventFlow is a .NET solution that demonstrates the principles of
**Event‑Driven Design (EDD)** using a **custom in‑memory event broker**.

The solution simulates a **decoupled, message‑based architecture**
where:

-   **Producers publish events**
-   **Consumers subscribe and react to them**
-   Everything runs **without external dependencies** such as RabbitMQ,
    Kafka, or Azure Service Bus.

This project is ideal for:

-   Learning event‑driven architecture
-   Prototyping distributed systems
-   Understanding how pub/sub messaging works internally

------------------------------------------------------------------------

# 🏗️ Solution Architecture

    Solution 'EventFlow'
    │
    ├── Consumers
    │   ├── Consumer1          # Worker service that subscribes and handles events
    │   └── Consumer2          # Another independent worker service consuming events
    │
    ├── Contracts
    │   └── Contract           # Shared event/message contracts (DTOs)
    │
    ├── EventBroker
    │   └── EventMessaging     # Core in‑memory event broker infrastructure
    │
    └── Producers
        └── ApiProducer1       # ASP.NET Core Web API that publishes events

------------------------------------------------------------------------

# 📦 Projects / Layers

## 1️⃣ EventBroker / EventMessaging

**Type:** Class Library

This is the **core of the system**: an in‑memory event broker that
simulates the behavior of a real message broker.

### Responsibilities

-   Maintains a registry of event subscriptions
-   Routes published events to registered handlers
-   Simulates brokers like RabbitMQ or Kafka
-   Works entirely **in‑memory**
-   Registered via **Dependency Injection (DI)**

This layer acts as the **central messaging backbone** of the system.

------------------------------------------------------------------------

## 2️⃣ Contracts / Contract

**Type:** Class Library

This project defines the **shared event contracts (DTOs)** used across
producers and consumers.

Both producers and consumers reference this project to ensure
**consistent and strongly‑typed event schemas**.

### Example Event

``` csharp
public class OrderCreated
{
    public Guid OrderId { get; set; }
}
```

### Why isolate contracts?

Separating contracts ensures:

-   Loose coupling
-   Independent services
-   Clear event schemas

Consumers and producers depend **only on contracts**, not on each other.

------------------------------------------------------------------------

## 3️⃣ Producers / ApiProducer1

**Type:** ASP.NET Core Web API

This project acts as the **event producer**.

It exposes HTTP endpoints that trigger business logic and **publish
events** to the event broker.

### Key Files

  ------------------------------------------------------------------------------------
  File                             Description
  -------------------------------- ---------------------------------------------------
  Controllers/OrderController.cs   API endpoints (e.g. POST /api/orders)

  Interfaces/IOrderService.cs      Abstraction for the order service

  Services/OrderService.cs         Business logic + event publishing

  ApiProducer1.http                File for testing endpoints

  Program.cs                       Application startup & DI configuration

  appsettings.json                 Application configuration
  ------------------------------------------------------------------------------------

### Flow

    HTTP Request
        ↓
    OrderController
        ↓
    OrderService
        ↓
    Publish Event (OrderCreated)
        ↓
    Event Broker

------------------------------------------------------------------------

## 4️⃣ Consumers / Consumer1

**Type:** .NET Worker Service

Consumer1 is a **background service** that subscribes to events and
processes them.

### Responsibilities

-   Listen for events published to the broker
-   Execute business logic when events arrive

### Key Files

  File               Description
  ------------------ ------------------------------------------
  Worker1.cs         Background service that processes events
  Program.cs         Application startup and DI configuration
  appsettings.json   Configuration

### Flow

    Event Broker (OrderCreated)
          ↓
    Worker1
          ↓
    Process Event

------------------------------------------------------------------------

## 5️⃣ Consumers / Consumer2

**Type:** .NET Worker Service

Consumer2 is a **second independent consumer** used to demonstrate the
**fan‑out pattern**.

Multiple consumers can react to the **same event independently**.

### Key Files

  File               Description
  ------------------ ------------------------------
  Worker2.cs         Independent event processing
  Program.cs         Startup configuration
  appsettings.json   Configuration

### Flow

    Event Broker (OrderCreated)
          ↓
    Worker2
          ↓
    Process Event

------------------------------------------------------------------------

# 🔄 End‑to‑End Event Flow

                              ┌──────────────────┐
                              │   ApiProducer1   │
                              │   (Web API)      │
                              │                  │
                              │ POST /api/orders │
                              └────────┬─────────┘
                                       │
                                 Publishes Event
                                   (OrderCreated)
                                       │
                                       ▼
                        ┌──────────────────────────┐
                        │      EventMessaging      │
                        │   In‑Memory Event Broker │
                        └─────────┬────────┬───────┘
                                  │        │
                                  ▼        ▼
                         ┌────────────┐ ┌────────────┐
                         │ Consumer1  │ │ Consumer2  │
                         │  Worker1   │ │  Worker2   │
                         └────────────┘ └────────────┘

This demonstrates the **fan‑out pattern**, where one event triggers
multiple consumers.

------------------------------------------------------------------------

# 🚀 Getting Started

## Prerequisites

-   .NET 8 SDK (or compatible version)
-   Visual Studio 2022 / JetBrains Rider / VS Code

------------------------------------------------------------------------

## Running the Solution

Because this is a **multi‑project solution**, you must run several
projects simultaneously.

### Configure Multiple Startup Projects

1.  Right‑click the **Solution**
2.  Select **Configure Startup Projects**
3.  Choose **Multiple startup projects**
4.  Set the following projects to **Start**:

-   ApiProducer1
-   Consumer1
-   Consumer2

Then press **F5**.

------------------------------------------------------------------------

## Trigger an Event

You can publish an event using:

-   The `ApiProducer1.http` file
-   Postman
-   curl

Example request:

    POST /api/orders

Both **Consumer1** and **Consumer2** will receive and process the event.

------------------------------------------------------------------------

# 🎯 Key Concepts Demonstrated

  Concept                     Description
  --------------------------- -----------------------------------------------
  Event‑Driven Architecture   Systems communicate through events
  Publish / Subscribe         Producers publish events; consumers subscribe
  Loose Coupling              Services are independent
  Fan‑Out                     One event can trigger multiple consumers
  Shared Contracts            Strongly‑typed communication
  In‑Memory Broker            No infrastructure required
  Separation of Concerns      Each project has a single responsibility

------------------------------------------------------------------------

# ⚠️ Disclaimer

This project uses an **in‑memory event broker** intended for:

-   Learning
-   Demonstration
-   Prototyping

For production systems, consider using real message brokers such as:

-   RabbitMQ
-   Apache Kafka
-   Azure Service Bus
-   MassTransit
