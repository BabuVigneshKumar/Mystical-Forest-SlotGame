# Mystical-Forest-SlotGame 
Slot Machine Game - Unity
A fully functional 3x5 slot machine game built with Unity, featuring 20 paylines, win detection, animations, and audio effects.

🎮 Features
3x5 Reel Layout with smooth spinning animations

20 Fixed Paylines with visual highlighting

Win Detection System for both regular symbols and scatters

Betting System with multiple bet levels (0.10 to 25.00)

Balance Management with initial balance of 2000

Audio System with sound effects for spins, wins, and errors

Loading Screen with progress bar

UI System with info panels and settings

Symbol Hierarchy (High, Low, Special symbols including Wild, Scatter, Bonus)

🛠️ Technical Implementation
Core Scripts
GameManager.cs - Main game controller handling betting, balance, and win detection

ReelsManager.cs - Manages reel spinning, symbol randomization, and animations

PaylineManager.cs - Handles payline validation and win checking

SlotData.cs - Individual slot symbol data and behavior

AudioController.cs - Manages all game audio effects

Key Systems
Symbol Database - Configurable symbol types with categories and payout multipliers

Payline Database - ScriptableObject-based payline configurations

PayTable System - Configurable payout values for different symbol combinations

DOTween Integration - Smooth animations for spins and wins

🎯 How to Play
Set Your Bet - Use +/- buttons to adjust bet amount (0.10 - 25.00)

Spin - Click the spin button to start the reels

Win Conditions:

3+ matching symbols on any payline (left to right)

Wild symbols substitute for all except Scatter/Bonus

3+ Scatter symbols anywhere on reels

Payouts - Based on symbol type and number of matches

📊 Symbol Payouts
Symbol Type	3 Symbols	4 Symbols	5 Symbols
Wild	1x	10x	20x
High 1	1x	10x	20x
High 2	0.8x	6x	16x
High 3	0.6x	4x	12x
High 4	0.4x	2x	8x
Low 1-5	0.3x	1.2x	3x
Scatter	3x10	4x10	5x10
🎨 Visual Features
Smooth Reel Animations with easing effects

Winning Symbol Highlighting with scale animations

Payline Visualization with LineRenderer effects

Loading Screen with progress animation

Snowfall Effect on background

Responsive UI with toggleable info panels

🔊 Audio Features
Reel Spin Sound during spinning

Reel Stop Sound when reels stop

Winning Sound for successful spins

Error Sound for unsuccessful spins

Button Click Sounds for UI interactions

🚀 Setup Instructions
Open in Unity - Requires Unity 2020.3 or later

Import DOTween - Ensure DOTween is imported for animations

Configure References - Set up scriptable objects for symbols and paylines

Audio Setup - Assign audio clips to AudioController

Build & Run - Test the game in editor or build for target platform

📁 Project Structure
