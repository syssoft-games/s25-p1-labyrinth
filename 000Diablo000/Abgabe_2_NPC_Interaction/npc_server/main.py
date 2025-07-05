from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
import httpx
import logging

# Logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger("npc-server")

# App starten
app = FastAPI()

# CORS aktivieren
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

# Ziel-URL für Ollama
OLLAMA_URL = "http://host.docker.internal:11434"

# Modellname
MODEL_NAME = "llama3"

# System-Prompt und Gesprächsverlauf
SYSTEM_PROMPT = {
    "role": "system",
    "content": (
        "Du bist ein NPC in einer Simulation. "
        "Du hast eine Kamera und kannst deine Umgebung wahrnehmen. "
        "Du bist neutral gegenüber anderen menschenähnlichen Wesen. "
        "Folgende Funktionen kannst du auslösen: vorwärts, rückwärts, links, rechts, springen, stoppen. "
        "Sprich freundlich und hilfsbereit. Reagiere auch, wenn man dir Anweisungen gibt."
    )
}
message_history = [SYSTEM_PROMPT]

# Eingabeformat
class PlayerInput(BaseModel):
    player_input: str

# POST /npc/respond
@app.post("/npc/respond")
async def respond(data: PlayerInput):
    user_input = data.player_input.strip()
    logger.info(f"🎮 Spieler sagt: {user_input}")

    # Verlauf aktualisieren
    message_history.append({"role": "user", "content": user_input})

    try:
        async with httpx.AsyncClient() as client:
            response = await client.post(
                f"{OLLAMA_URL}/api/chat",
                json={
                    "model": MODEL_NAME,
                    "messages": message_history,
                    "stream": False
                },
                timeout=60.0
            )
        response.raise_for_status()
    except Exception as e:
        logger.error(f"❌ Fehler bei Ollama: {e}")
        return {"reaction": "Idle", "say": "Ich konnte nicht antworten."}

    try:
        response_json = response.json()
        answer = response_json.get("message", {}).get("content", "").strip()
        message_history.append({"role": "assistant", "content": answer})
    except Exception as e:
        logger.error(f"❌ Antwort unklar: {e}")
        answer = "Ich bin verwirrt."

    logger.info(f"🤖 NPC sagt: {answer}")

    # Reaktionslogik
    reaction = "Idle"
    answer_lower = answer.lower()
    if "spring" in answer_lower:
        reaction = "Jump"
    elif "geh" in answer_lower or "lauf" in answer_lower or "vorwärts" in answer_lower:
        reaction = "Forward"
    elif "stop" in answer_lower or "halt" in answer_lower:
        reaction = "Stop"
    elif "zurück" in answer_lower:
        reaction = "Backward"
    elif "links" in answer_lower:
        reaction = "Left"
    elif "rechts" in answer_lower:
        reaction = "Right"

    return {
        "reaction": reaction,
        "say": answer
    }