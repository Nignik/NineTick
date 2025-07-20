# Ultimate Tic‑Tac‑Toe

A feature‑incomplete implementation of **Ultimate Tic‑Tac‑Toe** built with the Godot Engine.  

## 🌐 Online Multiplayer

It is possible to play online, to do that you need to run the server <https://github.com/Nignik/NineTickServer>

## 📜 Rulebook

You are given a 3 × 3 grid of **macro‑boards**.  
Each macro‑board contains its own 3 × 3 **micro‑board**.  
Players **black** and **white** alternate turns, starting with **white**.

On your turn you must select a tile on the micro-board.

The mandated micro‑board for turn *t* is determined by the cell chosen on turn *t − 1*.  
If that micro‑board is already *resolved* (won or full), the next player may choose **any unresolved micro‑board**.
First player can start on any micro-board

A **macro‑board** is won by the first player to align three marks (row, column, or diagonal) *within its micro‑board*.  
The entire game is won by the first player to align three won macro‑boards in the overall 3 × 3 grid.

If every micro‑board is resolved without a macro‑winner, the game is a **draw**.
