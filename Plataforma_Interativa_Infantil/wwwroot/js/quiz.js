// Conteúdo para: wwwroot/js/quiz.js

document.addEventListener('DOMContentLoaded', () => {
    // --- 1. Obter Elementos da DOM ---
    const quizDataContainer = document.getElementById('quiz-data-container');
    const formToken = document.getElementById('quiz-form-token');
    
    // Telas
    const activeScreen = document.getElementById('quiz-active-screen');
    const finalScreen = document.getElementById('final-screen');

    // Barra de Progresso
    const progressText = document.getElementById('progress-text');
    const progressPercent = document.getElementById('progress-percent');
    const progressBar = document.getElementById('progress-bar');

    // Card da Questão
    const perguntaTexto = document.getElementById('pergunta-texto');
    const feedbackBox = document.getElementById('feedback-box');
    const alternativasGrid = document.getElementById('alternativas-grid');
    const btnProxima = document.getElementById('btnProxima');

    // Tela Final
    const acertosFinais = document.getElementById('acertos-finais');
    const notaFinal = document.getElementById('nota-final');
    const tempoFinal = document.getElementById('tempo-final');
    const feedbackFinalEl = document.getElementById('feedback-final');

    // --- 2. Verificar se os dados existem ---
    if (!quizDataContainer || !quizDataContainer.dataset.quiz) {
        console.error('Dados do quiz não encontrados.');
        return;
    }

    // --- 3. Variáveis de Estado do Quiz ---
    const quizData = JSON.parse(quizDataContainer.dataset.quiz);
    const questoes = quizData.questoes;
    const totalQuestoes = questoes.length;
    let questaoAtualIndex = 0;
    let acertos = 0;
    let quizStartTime;

    // --- 4. Funções do Quiz ---

    /** Inicia o quiz e renderiza a primeira questão */
    function iniciarQuiz() {
        quizStartTime = new Date();
        renderizarQuestao(questaoAtualIndex);
    }

    /** Renderiza uma questão na tela */
    function renderizarQuestao(index) {
        // Limpa estados anteriores
        feedbackBox.classList.add('hidden');
        btnProxima.classList.add('hidden');
        alternativasGrid.innerHTML = '';

        // Pega a questão atual
        const questao = questoes[index];
        const progresso = Math.round(((index + 1) / totalQuestoes) * 100);

        // Atualiza UI
        perguntaTexto.innerText = questao.pergunta;
        progressText.innerText = `Questão ${index + 1} de ${totalQuestoes}`;
        progressPercent.innerText = `${progresso}%`;
        progressBar.style.width = `${progresso}%`;
        progressBar.setAttribute('aria-valuenow', progresso);

        // Cria os botões de alternativa
        questao.alternativas.forEach(alt => {
            const alternativaEl = document.createElement('div');
            alternativaEl.className = 'alternativa-card';
            alternativaEl.innerText = alt.texto;
            // Armazena o status de 'correta' no elemento
            alternativaEl.dataset.correta = alt.correta; 
            alternativaEl.addEventListener('click', onAlternativaClick);
            alternativasGrid.appendChild(alternativaEl);
        });
    }

    /** Chamado quando uma alternativa é clicada */
    function onAlternativaClick(e) {
        const selecionada = e.target;
        const isCorrect = selecionada.dataset.correta === 'true';

        // Desabilita todos os botões
        document.querySelectorAll('.alternativa-card').forEach(btn => {
            btn.classList.add('disabled');
            btn.removeEventListener('click', onAlternativaClick);
        });

        // Mostra feedback
        feedbackBox.classList.remove('hidden');
        if (isCorrect) {
            selecionada.classList.add('correct');
            feedbackBox.innerText = '✓ Resposta Correta! Muito bem!';
            feedbackBox.className = 'feedback-box correct';
            acertos++;
        } else {
            selecionada.classList.add('incorrect');
            feedbackBox.innerText = '✗ Resposta Incorreta.';
            feedbackBox.className = 'feedback-box incorrect';
            
            // Destaca a correta
            const corretaEl = alternativasGrid.querySelector('[data-correta="true"]');
            if (corretaEl) {
                corretaEl.classList.add('correct');
            }
        }

        // Mostra o botão "Próxima"
        btnProxima.classList.remove('hidden');
    }

    /** Vai para a próxima questão ou finaliza o quiz */
    function proximaQuestao() {
        questaoAtualIndex++;
        if (questaoAtualIndex < totalQuestoes) {
            renderizarQuestao(questaoAtualIndex);
        } else {
            finalizarQuiz();
        }
    }

    /** Calcula e exibe a tela final */
    function finalizarQuiz() {
        const quizEndTime = new Date();
        const tempoTotalSeg = Math.round((quizEndTime - quizStartTime) / 1000);
        const notaPercent = Math.round((acertos / totalQuestoes) * 100);

        // Formata o tempo
        const minutos = Math.floor(tempoTotalSeg / 60);
        const segundos = tempoTotalSeg % 60;
        const tempoFormatado = (minutos > 0 ? `${minutos}m ` : '') + `${segundos}s`;

        // Atualiza a tela final
        acertosFinais.innerText = `${acertos}/${totalQuestoes}`;
        notaFinal.innerText = `${notaPercent}%`;
        tempoFinal.innerText = tempoFormatado;
        
        // Define o feedback final
        if (notaPercent >= 80) {
            feedbackFinalEl.innerText = "Excelente! Você é um gênio!";
        } else if (notaPercent >= 50) {
            feedbackFinalEl.innerText = "Muito bem! Continue assim!";
        } else {
            feedbackFinalEl.innerText = "Bom esforço! Continue praticando.";
        }

        // Mostra a tela final e esconde a de quiz
        activeScreen.classList.add('hidden');
        finalScreen.classList.remove('hidden');

        // Dispara confetes!
        confetti({
            particleCount: 150,
            spread: 90,
            origin: { y: 0.6 }
        });

        // Envia o resultado para o servidor
        enviarResultado(notaPercent);
    }

    /** Envia o resultado para o Controller */
    async function enviarResultado(nota) {
        // Pega o token do formulário
        const token = formToken.querySelector('input[name="__RequestVerificationToken"]').value;

        const resultado = {
            atividadeId: quizData.id,
            nota: nota,
            acertos: acertos,
            totalQuestoes: totalQuestoes
        };

        try {
            await fetch('/api/atividades/salvarresultado', { 
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(resultado)
            });
        } catch (err) {
            console.error('Erro ao salvar o resultado:', err);
        }
    }

    // --- 5. Iniciar ---
    btnProxima.addEventListener('click', proximaQuestao);
    iniciarQuiz();
});