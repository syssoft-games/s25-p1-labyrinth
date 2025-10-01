🧩 Exercise 1 & 2 – 3D Labyrinth and NPC AI with Ray Tracing
Link to Presi: https://www.dropbox.com/scl/fi/5vye8ndz6fhfks8il72xc/NPC_Presi_2.mov?rlkey=sc0g9dul9hvr7vvv7gr8vjwkg&st=sgjw3yyv&dl=0 
👨‍🎓 Author:   Narek Grigoryan
📅 Semester: Sommer 2025
🎮 Course:   Spieleprogrammierung (Prof. Sturm & Tihomir Bicanic)
📁 Folder:   NarekGrigoryan


🧠 Project Overview

This Unity project is my submission for Exercise 1 and Exercise 2 in the context of the Spieleprogrammierung course at the University of Trier.

It demonstrates:
	•	A 3D labyrinth scene with ray tracing
	•	Interactive navigation with a first-person controller
	•	Realistic materials: reflective, transparent, absorbent
	•	An AI-controlled NPC with context-aware behavior using a language model (LLM)

🧱 Structure & Setup

The core of the project is inside the folder:
NarekGrigoryan/

It includes:
	•	All Unity project files (Assets, Packages, ProjectSettings)
	•	A labyrinth.json file describing the 2D structure of the maze
	•	NPC character with animations (walking, running, crawling, idle, hiding)
	•	Two PDF reports:
	•	Spielprogrammierung_Uebung_1.pdf
	•	Spielprogrammierung_Uebung_2.pdf

🔧 To run the project:
	1.	Clone the repository
	2.	Open the project with Unity 6.x (HDRP)
	3.	Open the scene: Assets/OutdoorsScene/OutdoorsScene.unity
	4.	Press Play to explore the labyrinth and interact with the NPC

🤖 About the AI

The NPC behavior is controlled by a custom script that connects to a local LLM API (e.g., LM Studio).
The model receives contextual information (e.g., player distance, actions) and responds with commands like:
	•	0 for Idle
	•	1 for Walk
	•	2 for Run
	•	3 for Crawl
	•	4 for Kick (Hide)

⚠️ Due to time constraints and server issues, some planned improvements (like richer prompts or dynamic context handling) could not be completed.


📄 Reports

Detailed documentation is available in:
	•	Spielprogrammierung_Uebung_1.pdf
	•	Spielprogrammierung_Uebung_2.pdf

These reports explain the technical implementation, design decisions, and challenges faced during development.


✅ Final Notes

This submission includes only the features that were fully functional and tested.
The AI integration is modular and can be expanded further if more time and compute resources are available.
