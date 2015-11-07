# Commando
Command line parsing inspired by [Commander.js](https://github.com/tj/commander.js)

### Usage
```csharp
public static void Main (string[] args)
{
  dynamic program = new Commando()
    .Version("0.0.1")
    .Parameter ("p", "pizza", "Some pizza", true)
    .Parameter ("d", "drink", "Some drink", true)
    .Switch ("v", "vegetables", "Want vegetables?", false)
    .Parse ("-p Capricciosa -d Coke -v");

  if (program.pizza != "Margherita")
    Console.WriteLine("We only serve margheritas!");
  if (program.vegetables)
    Console.WriteLine("We're out of vegetables!");

  Console.WriteLine (String.Format ("You've ordered a {0} with {1}",
    program.pizza,
    program.drink));
}
```
Will output
```
We only serve margheritas!
We're out of vegetables!
You've ordered a Capricciosa with Coke
```

### License
MIT
