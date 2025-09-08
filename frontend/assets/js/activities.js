let currentActivity = null;
let score = 0;
let correctAnswers = 0;
let totalQuestions = 0;

// Buscar atividades do backend
async function loadActivities(category) {
    const res = await fetch(`/api/activities?category=${category}`);
    const activities = await res.json();

    if (activities.length > 0) {
        currentActivity = activities[0]; // pega a primeira da categoria
        totalQuestions = activities.length;
        showActivity(currentActivity);
    }
}

// Mostrar uma atividade
function showActivity(activity) {
    document.getElementById("welcomeScreen").classList.add("hidden");
    document.getElementById("activityScreen").classList.remove("hidden");

    document.getElementById("questionText").textContent = activity.title;
    document.getElementById("answersGrid").innerHTML = "";

    activity.options.forEach(option => {
        const btn = document.createElement("button");
        btn.classList.add("btn", "btn-outline-primary", "answer-btn");
        btn.innerHTML = option.text;
        btn.onclick = () => checkAnswer(activity.id, option);
        document.getElementById("answersGrid").appendChild(btn);
    });
}

// Verificar resposta e enviar para o backend
async function checkAnswer(activityId, option) {
    const correct = option.isCorrect;
    const feedbackScreen = document.getElementById("feedbackScreen");
    const feedbackContent = document.getElementById("feedbackContent");

    if (correct) {
        score += 10;
        correctAnswers++;
        document.getElementById("successSound").play();
        feedbackContent.innerHTML = "<h2>🎉 Muito bem! Resposta correta!</h2>";
    } else {
        document.getElementById("errorSound").play();
        feedbackContent.innerHTML = "<h2>❌ Ops! Não foi dessa vez.</h2>";
    }

    // registrar no backend
    await fetch("/api/responses", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            activityId: activityId,
            answer: option.text,
            isCorrect: correct
        })
    });

    document.getElementById("activityScreen").classList.add("hidden");
    feedbackScreen.classList.remove("hidden");
}

// Continuar para próxima pergunta
function nextActivity(activities, index) {
    if (index < activities.length) {
        showActivity(activities[index]);
    } else {
        showResults();
    }
}

// Mostrar tela final de resultados
function showResults() {
    document.getElementById("feedbackScreen").classList.add("hidden");
    document.getElementById("resultsScreen").classList.remove("hidden");

    document.getElementById("finalScore").textContent = score;
    document.getElementById("correctAnswers").textContent = correctAnswers;
    document.getElementById("totalQuestions").textContent = totalQuestions;

    // 🎊 confete
    startConfetti();
}
