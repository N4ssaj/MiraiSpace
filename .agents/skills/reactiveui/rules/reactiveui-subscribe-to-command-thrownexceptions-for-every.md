---
title: "Configure the common ReactiveUI command exception handler"
impact: MEDIUM
impactDescription: "general best practice"
tags: reactiveui, dotnet, reactive, building-mvvm-applications-using-reactive-extensions-with-reactiveobject, whenanyvalue, reactivecommand
---

## Configure the common ReactiveUI command exception handler

Configure ReactiveUI's common command exception handler at bootstrap. In MiraiSpace, do not add repetitive catches or subscribe to every command's `ThrownExceptions` for logging. Local subscriptions are reserved for deliberate feature-specific recovery; explicitly forward/report failures that still need the common policy. Verify the bootstrap API against the installed version.

A local observer can prevent errors from reaching the default handler, so forwarding/reporting must be deliberate. Expected cancellation handling belongs to the agreed common policy instead of scattered empty catches. The command handler does not cover arbitrary exceptions elsewhere in the process.
