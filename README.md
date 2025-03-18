# Roulette Game Case Study

This project is a case study for Joker Games, prepared by Yusuf Metindoğan. The objective is to develop a 3D deterministic roulette prototype in Unity that demonstrates a modular coding structure designed for ease of maintenance and unit testing using Unity's TestRunner.

## Unity Packages Used
- ProBuilder
- 2D Sprite Editor

## Development Tools
- **Unity Version:** 2022.3.20f1  
- **IDE:** JetBrains Rider  
- **UI Design:** Figma (for designing UI images and sprites)

## Data Storage
The application stores PlayerData and StatisticsData in:
%AppData%\LocalLow\YusufMetindogan

## Modular Structure
The project is divided into several modules:
- **Roulette Module:** Handles the roulette wheel and ball animation with deterministic outcomes.
- **Betting Module:** Manages bet placement, validation, and payout calculations along with visual chip placement.
- **Player Module:** Manages player data (money balance) and uses JSON for persistence.
- **Statistics Module:** Tracks and displays play history and total balance.

This modular design was chosen to simplify testing using Unity's TestRunner and to ensure a clean separation of concerns.

## External Assets Licensing
- **Roulette Table Model:** [https://sketchfab.com/3d-models/roulette-table-5a881735972441a084fe71e0424df60b](https://sketchfab.com/3d-models/roulette-table-5a881735972441a084fe71e0424df60b)
- **Casino Chips Model:** [https://www.cgtrader.com/free-3d-models/various/various-models/casino-chips-4a5f0873-833c-4c56-b799-671832e39e0a](https://www.cgtrader.com/free-3d-models/various/various-models/casino-chips-4a5f0873-833c-4c56-b799-671832e39e0a)
- **History Icon:** [https://www.freepik.com/icon/clock-icon_11720326](https://www.freepik.com/icon/clock-icon_11720326)

## Room for Improvement
- **Sound Effects:**  
  Time constraints prevented integrating sound effects. Future work would include proper audio feedback for spins, bets, and wins.
- **Particle Effects & Lighting:**  
  No particle effects or advanced lighting were implemented due to focus on structure and modular coding. This could be added to enhance the visual polish.
- **Additional Betting Options:**  
  Further refinement of bet types and validations could be explored.
- **UI Animations:**  
  More sophisticated UI transitions and animations (beyond the basic flicker and oscillation) can be implemented.
- **Extended Testing:**  
  Although the modular structure was designed with testing in mind, more extensive unit tests and integration tests would improve robustness.
