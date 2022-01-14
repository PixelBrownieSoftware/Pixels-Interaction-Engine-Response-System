# Pixel's Interaction Engine Response System
 PIERS
This is a dialogue system, heavily inspired by Valve's repsonse system (https://www.youtube.com/watch?v=tAbBID3N64A).

## How it works
Each character in a scene has a 'dialogue speaker' and a collection of facts and rules. When an event (with some facts to establish context) is dispatched to a character, they check their database of rules and asses which ones fit the context and say it. Once the dialogue has been said, they may modify their own facts and or do a followup response to another character.

## Features

### Rules
These are conditiions which contain a peice of dialouge. They are checked against facts and are picked out based on which facts meet the critiera. The rules with the most critiera are picked first. Rules are sorted out by a special fact named a 'Concept' which is basically an event.

### Facts/Context
These are what are checked in the rules. They can be strings or numbers

## Planned Features

### Speech context
These are objects that track speakers and listeners. When an event is dispatched, speakers and listeners are added to this context. The purpose of this object is to avoid non-essential interruption to the listeners.

### Different types of events
For now, this system only has dialogue. In the future I would like to expand this system to more than just dialogue i.e. movement, choices etc.
