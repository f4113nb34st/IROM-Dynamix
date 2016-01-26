# IROM-Dynamix

Dynamix is a system of dynamic variables that "remember" their definition and update whenever their inputs change. Each variable is a readonly object with two properties, Value and Exp. 

1. Value can be used to get the current value of the Dynamix variable, or it can be used to set the variable to a constant.
2. Exp, or expression, is a zero args function that returns the current value of the variable. It can be used to set a variable to a dynamic value. Subscription to Dynamix parents is automatically handled within the execution of this function.

For example, if i and j are both Dynamix ints:
i.Value = 5;
j.Exp = () => i.Value + 6;
i.Value = 7;
//j.Value == 13

Furthermore, Dynamix variables have an event called OnFilter. This event allows the output of a Dynamix var to be filtered before the final result is set. This is useful for cases with constrained acceptible outputs, such as a font size that must be greater than zero.

Dynamix variables can be used in complicated, nested situations as well. A Dynamix expression can include an if statement based on a Dynamix bool that returns one of two Dynamix ints, and subscription and resubscription is handled automatically, with minimum parent subscription. (This means the variable won't be subscribed to the variable on the other side of the if branch until the branch actually flips.)

Finally, Dynamix is fast, small, and lightweight.
SPEED: Updates are performed at rates exceeding 750000 updates per second.
SIZE: The entire package is only 7 classes and the binaries are only 28kb.
MEMORY: Each Dynamix variable only requires about 38 words of memory (*depending on usage)

Overall, Dynamix variables allow previously difficult dynamic tasks to be handled easily, like automatic gui localization updating, or major gui restructuring based on a config variable-- All with no manual event handling code.
