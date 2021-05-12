# Abel.PropertyInjection

Property injection is the answer to the question: "Why do I need to define fields or properties for my dependencies, then have them injected in a constructor, and then assign them? I don't want to repeat myself thrice, what can I do?!"

This is a typical example:

~~~
private readonly IEnvironmentRepository _environmentRepository;

private readonly IModelMapper<GroupServiceModel, GroupEntity> _groupMapper;

public EnvironmentService(
	IEnvironmentRepository environmentRepository,
	IModelMapper<GroupServiceModel, GroupEntity> groupMapper)
{
    _environmentRepository = environmentRepository;
    _groupMapper = groupMapper;
}
~~~

With property injection, that block of code is replaced with:

~~~
[Inject]
private readonly IEnvironmentRepository _environmentRepository;

[Inject]
private readonly IModelMapper<GroupServiceModel, GroupEntity> _groupMapper;
~~~

That's right, constructors are a thing of the past. Just slap some `[Inject]` attributes on there and you're good to go.

## Supported member types

The observant reader might notice that the above example uses fields, not properties. Fields work just as well, property injection just sounds cooler. The following member types are supported:

* Properties with public setters
* Properties with private setters
* Properties without any setter
* Public fields
* Protected fields
* Private fields
* Readonly fields

## What about inheritance?

You can remove constructors all the way up the chain: base classes, children and everything in-between.

## Installation

1. Download the Nuget package.
2. Add `.UsePropertyInjection()` to your host builder. If you don't have one, you can follow [this guide](https://dfederm.com/building-a-console-app-with-.net-generic-host/) to add it.

~~~
Host.CreateDefaultBuilder(args)
    .UsePropertyInjection()
    .ConfigureWebHostDefaults(webBuilder =>
		webBuilder.UseStartup<Startup>());
~~~

3. Delete your constructors.
4. Add `[Inject]` attributes to your fields and properties.