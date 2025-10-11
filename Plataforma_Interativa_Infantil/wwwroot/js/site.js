document.addEventListener('DOMContentLoaded', function () {
   
    const atividadeBody = document.getElementById('atividade-body');
    const finalScreen = document.getElementById('final-screen');
    const progressBar = document.getElementById('progress-bar');
    const progressText = document.getElementById('progress-text');
    const perguntaTitulo = document.getElementById('pergunta-titulo');
    const alternativasContainer = document.getElementById('alternativas-container');
    const feedbackFooter = document.getElementById('feedback-footer');
    const feedbackTexto = document.getElementById('feedback-texto');
    
 
    const acertosFinais = document.getElementById('acertos-finais');
    const notaFinal = document.getElementById('nota-final');
    const tempoFinal = document.getElementById('tempo-final');

   
    if (!atividadeBody || !atividadeData) {
    
        return;
    }

    const questoes = atividadeData.questoes;
    const totalQuestoes = questoes.length;
    let currentQuestionIndex = 0;
    let acertos = 0;

   
    let startTime;

    function renderQuestion(index) {
     
        if (index === 0) {
            startTime = new Date();
        }

        feedbackFooter.classList.remove('visible');
        perguntaTitulo.classList.remove('fade-out');
        alternativasContainer.classList.remove('fade-out');

        const questao = questoes[index];
        const progresso = ((index + 1) / totalQuestoes) * 100;

        perguntaTitulo.innerText = questao.pergunta;
        progressText.innerText = `Questão ${index + 1} de ${totalQuestoes}`;
        progressBar.style.width = `${progresso}%`;

        alternativasContainer.innerHTML = '';
        questao.alternativas.forEach(alt => {
            const alternativaEl = document.createElement('div');
            alternativaEl.className = 'alternativa-card';
            alternativaEl.innerText = alt.texto;
            alternativaEl.dataset.correta = alt.correta;
            alternativaEl.addEventListener('click', handleAnswerClick);
            alternativasContainer.appendChild(alternativaEl);
        });
    }

    function handleAnswerClick(event) {
        const selecionada = event.target;
        const isCorrect = selecionada.dataset.correta.toLowerCase() === 'true';

        document.querySelectorAll('.alternativa-card').forEach(btn => {
            btn.classList.add('disabled');
            btn.removeEventListener('click', handleAnswerClick);
        });

        if (isCorrect) {
            selecionada.classList.add('correct');
            feedbackTexto.innerText = "🎉 Resposta Correta!";
            feedbackFooter.className = 'feedback-footer correct visible';
            acertos++;
        } else {
            selecionada.classList.add('incorrect');
            feedbackTexto.innerText = "❌ Ops! Resposta errada.";
            feedbackFooter.className = 'feedback-footer incorrect visible';
            
            const corretaEl = document.querySelector('.alternativa-card[data-correta="true"]');
            if (corretaEl) corretaEl.classList.add('correct');
        }

        setTimeout(() => proximaQuestao(), 2000);
    }

    function proximaQuestao() {
        feedbackFooter.classList.remove('visible');
        perguntaTitulo.classList.add('fade-out');
        alternativasContainer.classList.add('fade-out');

        currentQuestionIndex++;

        setTimeout(() => {
            if (currentQuestionIndex < totalQuestoes) {
                renderQuestion(currentQuestionIndex);
            } else {
                showFinalScreen();
            }
        }, 400);
    }

    function formatarTempo(totalSegundos) {
        const minutos = Math.floor(totalSegundos / 60);
        const segundos = Math.floor(totalSegundos % 60);
        return `${minutos}m ${segundos}s`;
    }

    async function showFinalScreen() {
        const endTime = new Date();
        const tempoDecorrido = (endTime - startTime) / 1000; 
        const notaPercentual = Math.round((acertos / totalQuestoes) * 100);

      
        acertosFinais.innerText = `${acertos}/${totalQuestoes}`;
        notaFinal.innerText = `${notaPercentual}%`;
        tempoFinal.innerText = formatarTempo(tempoDecorrido);
        
        
        document.getElementById('atividade-header').style.display = 'none';
        atividadeBody.style.display = 'none'; 
        finalScreen.classList.remove('d-none'); 
        
       
        confetti({ particleCount: 200, spread: 100, origin: { y: 0.6 } });

        try {
            await fetch('/api/atividades/salvarresultado', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    atividadeId: atividadeData.id,
                    acertos: acertos,
                    totalQuestoes: totalQuestoes
                })
            });
        } catch (err) {
            console.error('Erro ao salvar o resultado:', err);
        }
    }
    
   
    if (totalQuestoes > 0) {
        renderQuestion(currentQuestionIndex);
    }

const accessBtn = document.getElementById("accessBtn");
const accessMenu = document.getElementById("accessMenu");

if (accessBtn && accessMenu) {
    accessBtn.addEventListener("click", () => {
        accessMenu.style.display = accessMenu.style.display === "block" ? "none" : "block";
    });
}

function toggleContrast() {
    document.body.classList.toggle("high-contrast");
}

function toggleFont() {
    document.body.classList.toggle("large-font");
}

function toggleDaltonismo() {
    document.body.classList.toggle("daltonismo");
}

function toggleAnim() {
    const style = document.createElement("style");
    style.innerHTML = "* { animation: none !important; transition: none !important; }";
    document.head.appendChild(style);
}

function toggleRead() {
    const text = document.body.innerText;
    const utterance = new SpeechSynthesisUtterance(text);
    speechSynthesis.speak(utterance);
}
const contrastBtn = document.getElementById("contrastBtn");
const fontBtn = document.getElementById("fontBtn");
const daltonismoBtn = document.getElementById("daltonismoBtn");
const animBtn = document.getElementById("animBtn");
const readBtn = document.getElementById("readBtn");

if (daltonismoBtn) {
    daltonismoBtn.addEventListener("click", toggleDaltonismo);
}

if (animBtn) {
    animBtn.addEventListener("click", toggleAnim);
}

if (readBtn) {
    readBtn.addEventListener("click", toggleRead);
}
if (contrastBtn) {
    contrastBtn.addEventListener("click", toggleContrast);
}
if (fontBtn) {
    fontBtn.addEventListener("click", toggleFont);
}
});