document.addEventListener('DOMContentLoaded', function () {
    const atividadeBody = document.getElementById('atividade-body');
    const finalScreen = document.getElementById('final-screen');
    const acertosFinais = document.getElementById('acertos-finais');
    const progressBar = document.getElementById('progress-bar');
    const progressText = document.getElementById('progress-text');
    const perguntaTitulo = document.getElementById('pergunta-titulo');
    const alternativasContainer = document.getElementById('alternativas-container');
    const feedbackFooter = document.getElementById('feedback-footer');
    const feedbackTexto = document.getElementById('feedback-texto');
    // 'btnContinuar' foi removido.

    const questoes = atividadeData.questoes;
    const totalQuestoes = questoes.length;
    let currentQuestionIndex = 0;
    let acertos = 0;

    function renderQuestion(index) {
        feedbackFooter.classList.add('d-none');
        alternativasContainer.innerHTML = '';

        const questao = questoes[index];
        const progresso = ((index + 1) / totalQuestoes) * 100;

        perguntaTitulo.innerText = questao.pergunta;
        progressText.innerText = `Questão ${index + 1} de ${totalQuestoes}`;
        progressBar.style.width = `${progresso}%`;

        questao.alternativas.forEach(alt => {
            const alternativaEl = document.createElement('div');
            alternativaEl.className = 'alternativa-card';
            alternativaEl.innerText = alt.texto;
            alternativaEl.dataset.correta = alt.correta;
            alternativasContainer.appendChild(alternativaEl);
            alternativaEl.addEventListener('click', handleAnswerClick);
        });
    }

    function animacaoAcerto() {
        document.body.classList.add('bg-correct');
        setTimeout(() => document.body.classList.remove('bg-correct'), 800);
    }

    function animacaoErro() {
        document.body.classList.add('bg-incorrect');
        setTimeout(() => document.body.classList.remove('bg-incorrect'), 800);
    }


    function handleAnswerClick(event) {
        const selecionada = event.target;
        const isCorrect = selecionada.dataset.correta.toLowerCase() === 'true';

        // Desabilita todas as alternativas
        document.querySelectorAll('.alternativa-card').forEach(btn => {
            btn.classList.add('disabled');
            btn.removeEventListener('click', handleAnswerClick);
        });

        if (isCorrect) {
            selecionada.classList.add('correct');
            feedbackTexto.innerText = "🎉 Resposta Correta!";
            feedbackFooter.className = 'feedback-footer correct';
            acertos++;
            animacaoAcerto();
        } else {
            selecionada.classList.add('incorrect');
            feedbackTexto.innerText = "❌ Ops! Tente na próxima!";
            feedbackFooter.className = 'feedback-footer incorrect';
            animacaoErro();
            // Marca a correta
            const corretaEl = document.querySelector('.alternativa-card[data-correta="true"]');
            if (corretaEl) corretaEl.classList.add('correct');
        }
        
        // 1. MOSTRA O FEEDBACK
        feedbackFooter.classList.remove('d-none');
        
        // 2. FLUXO AUTOMÁTICO: AVANÇA APÓS 2 SEGUNDOS
        setTimeout(() => {
            proximaQuestao();
        }, 2000); 
    }

    function proximaQuestao() {
        // Oculta o rodapé de feedback antes de avançar
        feedbackFooter.classList.add('d-none');

        currentQuestionIndex++;
        if (currentQuestionIndex < totalQuestoes) {
            renderQuestion(currentQuestionIndex);
        } else {
            showFinalScreen();
        }
    }

    async function showFinalScreen() {
        atividadeBody.classList.add('d-none');
        feedbackFooter.classList.add('d-none');
        finalScreen.classList.remove('d-none');
        acertosFinais.innerText = acertos;

        confetti({ particleCount: 150, spread: 90, origin: { y: 0.6 } });

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

    renderQuestion(currentQuestionIndex);

    // --- Acessibilidade ---
    const increaseFontBtn = document.getElementById('increaseFont');
    const decreaseFontBtn = document.getElementById('decreaseFont');
    const toggleContrastBtn = document.getElementById('toggleContrast');

    if (increaseFontBtn) {
        increaseFontBtn.addEventListener('click', () => changeFontSize(2));
    }
    if (decreaseFontBtn) {
        decreaseFontBtn.addEventListener('click', () => changeFontSize(-2));
    }
    if (toggleContrastBtn) {
        toggleContrastBtn.addEventListener('click', () => {
            document.body.classList.toggle('high-contrast');
        });
    }

    function changeFontSize(amount) {
        let currentSize = parseFloat(window.getComputedStyle(document.body).getPropertyValue('font-size'));
        document.body.style.fontSize = (currentSize + amount) + 'px';
    }
});