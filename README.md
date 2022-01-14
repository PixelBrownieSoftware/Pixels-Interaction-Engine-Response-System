# Pixel's Interaction Engine Response System
 PIERS
This is a dialogue system, heavily inspired by Valve's repsonse system (https://www.youtube.com/watch?v=tAbBID3N64A).

## How it works
Each character in a scene has a 'dialogue speaker' and a collection of facts and rules. 

## Features

### Rules
These are conditiions which contain a peice of dialouge. They are checked against facts and are picked out based on which facts meet the critiera. The rules with the most critiera are picked first. Rules are sorted out by a special fact named a 'Concept' which is basically an event.

### Facts/Context
These are what are checked in the rules. They can be strings or numbers
