![GitHub Workflow Status]https://img.shields.io/github/actions/workflow/status/zenonet/SlowLang/dotnet.yml?branch=master)
# SlowLang
SlowLang is a semi-eso programming language I designed just for fun.<br>
Syntactically, it's very simmilar to most modern languages such as C# or Java.<br>

Right now it doesn't have a lot of functionality. Basically everything you can do is infinite loops, input, output, getting the current Time and variables.

SlowLang is an interpreted language which is based on the [SlowLangEngine](https://github.com/zenonet/SlowLangEngine).<br>
The parser of SlowLangEngine is pretty extendable so if you want, you can create more cool statements and create a pull-request.<br>If you need help, just 
[create an Issue](https://github.com/zenonet/SlowLang/issues/new).

## Hello World

You can write a hello world script in SlowLang like this:

```c#
print("Hello World!");
```

The semicolon is optional but strongly recommended.

## While Loops

You can create a while loop like this:

```c#
while(true){
  print("Hello");
}
```
