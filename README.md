# Slot Machine Game - Unity

A fully functional 3×5 slot machine game built with Unity, featuring 20 paylines, win detection, smooth animations, and immersive audio effects.

![Unity](https://img.shields.io/badge/Unity-2020.3%2B-000000?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![DOTween](https://img.shields.io/badge/Animation-DOTween-green?style=for-the-badge)

## ✨ Features

### 🎰 Gameplay
- **3×5 Reel Layout** with smooth spinning animations
- **20 Fixed Paylines** with visual highlighting using LineRenderer
- **Win Detection System** for both regular symbols and scatters
- **Wild Symbol Mechanics** - substitutes for all symbols except Scatter/Bonus
- **Scatter Wins** - pays anywhere on reels

### 💰 Betting & Economy
- **Flexible Betting System** with multiple levels (0.10 to 25.00)
- **Balance Management** with initial balance of 2000
- **Configurable Payouts** based on symbol type and match count
- **Auto-reset** of bet amounts after each spin

### 🎨 Visual & Audio
- **Smooth Animations** powered by DOTween
- **Winning Symbol Highlighting** with scale pulses
- **Payline Visualization** with animated LineRenderer effects
- **Loading Screen** with progress bar animation
- **Snowfall Background Effect**
- **Comprehensive Audio System** with sounds for spins, wins, and errors

### 🎯 Symbol System
- **Symbol Hierarchy**: High, Low, and Special categories
- **Special Symbols**: Wild, Scatter, and Bonus
- **Configurable Database** using ScriptableObjects

## 🚀 How to Play

1. **Set Your Bet** - Use the +/- buttons to adjust your bet amount (0.10 - 25.00)
2. **Spin the Reels** - Click the spin button to start the game
3. **Watch for Wins**:
   - 3+ matching symbols on any payline (left to right)
   - Wild symbols substitute for regular symbols
   - 3+ Scatter symbols anywhere on reels trigger scatter wins
4. **Collect Winnings** - Payouts are automatically added to your balance

## 📊 Payout Structure

| Symbol Type | 3 Symbols | 4 Symbols | 5 Symbols |
|-------------|-----------|-----------|-----------|
| Wild        | 1x        | 10x       | 20x       |
| High 1      | 1x        | 10x       | 20x       |
| High 2      | 0.8x      | 6x        | 16x       |
| High 3      | 0.6x      | 4x        | 12x       |
| High 4      | 0.4x      | 2x        | 8x        |
| Low 1-5     | 0.3x      | 1.2x      | 3x        |
| Scatter     | 30x       | 40x       | 50x       |

## 🛠️ Technical Architecture

### Core Scripts
- **`GameManager.cs`** - Main game controller (betting, balance, win detection)
- **`ReelsManager.cs`** - Reel spinning, symbol randomization, animations
- **`PaylineManager.cs`** - Payline validation and win checking
- **`SlotData.cs`** - Individual slot symbol behavior
- **`AudioController.cs`** - Audio management system

### Data Systems
- **Symbol Database** - Configurable symbol types and categories
- **Payline Database** - ScriptableObject-based payline configurations
- **PayTable System** - Customizable payout values

### Animation System
- **DOTween Integration** for smooth animations
- **Easing Effects** for natural movement
- **Sequenced Animations** for complex transitions

## 📦 Installation & Setup

### Prerequisites
- Unity 2022.3 or later
- DOTween plugin (import via Asset Store or Package Manager)

### Setup Steps
1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/slot-machine-game.git
