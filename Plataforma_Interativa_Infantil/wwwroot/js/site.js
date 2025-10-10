document.addEventListener('DOMContentLoaded', function () {
    // Seleção de Elementos
    const atividadeBody = document.getElementById('atividade-body');
    const finalScreen = document.getElementById('final-screen');
    const progressBar = document.getElementById('progress-bar');
    const progressText = document.getElementById('progress-text');
    const perguntaTitulo = document.getElementById('pergunta-titulo');
    const alternativasContainer = document.getElementById('alternativas-container');
    const feedbackFooter = document.getElementById('feedback-footer');
    const feedbackTexto = document.getElementById('feedback-texto');
    
    // Novos elementos da tela final
    const acertosFinais = document.getElementById('acertos-finais');
    const notaFinal = document.getElementById('nota-final');
    const tempoFinal = document.getElementById('tempo-final');

    // Verifica se os elementos essenciais existem antes de continuar
    if (!atividadeBody || !atividadeData) {
        // Se não estiver na página da atividade, não faz nada.
        return;
    }

    const questoes = atividadeData.questoes;
    const totalQuestoes = questoes.length;
    let currentQuestionIndex = 0;
    let acertos = 0;

    // Variáveis do Cronômetro
    let startTime;

    function renderQuestion(index) {
        // Inicia o cronômetro na primeira questão
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
        }, 400); // Espera a animação de fade-out
    }

    function formatarTempo(totalSegundos) {
        const minutos = Math.floor(totalSegundos / 60);
        const segundos = Math.floor(totalSegundos % 60);
        return `${minutos}m ${segundos}s`;
    }

    async function showFinalScreen() {
        const endTime = new Date();
        const tempoDecorrido = (endTime - startTime) / 1000; // em segundos
        const notaPercentual = Math.round((acertos / totalQuestoes) * 100);

        // Atualiza os valores na nova tela final
        acertosFinais.innerText = `${acertos}/${totalQuestoes}`;
        notaFinal.innerText = `${notaPercentual}%`;
        tempoFinal.innerText = formatarTempo(tempoDecorrido);
        
        // Esconde o corpo da atividade e mostra a tela final
        document.getElementById('atividade-header').style.display = 'none';
        atividadeBody.style.display = 'none'; // Esconde a área de perguntas
        finalScreen.classList.remove('d-none'); // Mostra a tela de resultados
        
        // Ativa os confetes
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
    
    // Inicia o jogo
    if (totalQuestoes > 0) {
        renderQuestion(currentQuestionIndex);
    }

    document.addEventListener('DOMContentLoaded', () => {
  let currentFontSize = 100;
  let leituraAtiva = false;
  let synth = window.speechSynthesis;

  const increaseFont = document.getElementById('increaseFont');
  const decreaseFont = document.getElementById('decreaseFont');
  const resetFont = document.getElementById('resetFont');
  const toggleContrast = document.getElementById('toggleContrast');
  const toggleDaltonismo = document.getElementById('toggleDaltonismo');
  const toggleLeitura = document.getElementById('toggleLeitura');
  const toggleAnimacoes = document.getElementById('toggleAnimacoes');

  // Aumentar Fonte
  increaseFont.addEventListener('click', () => {
    currentFontSize += 10;
    document.body.style.fontSize = `${currentFontSize}%`;
  });

  // Diminuir Fonte
  decreaseFont.addEventListener('click', () => {
    currentFontSize = Math.max(80, currentFontSize - 10);
    document.body.style.fontSize = `${currentFontSize}%`;
  });

  // Resetar Fonte
  resetFont.addEventListener('click', () => {
    currentFontSize = 100;
    document.body.style.fontSize = '100%';
  });

  // Alto Contraste
  toggleContrast.addEventListener('click', () => {
    document.body.classList.toggle('high-contrast');
  });

  // Modo Daltônico (simulação de protanopia)
  toggleDaltonismo.addEventListener('click', () => {
    document.body.classList.toggle('daltonismo');
  });

  // Leitura de Texto com Voz
  toggleLeitura.addEventListener('click', () => {
    leituraAtiva = !leituraAtiva;
    toggleLeitura.innerText = leituraAtiva ? 'Desativar Leitura de Texto' : 'Ativar Leitura de Texto';
    if (leituraAtiva) {
      document.addEventListener('click', lerTexto);
    } else {
      document.removeEventListener('click', lerTexto);
      synth.cancel();
    }
  });

  function lerTexto(e) {
    const texto = e.target.innerText || e.target.alt || '';
    if (texto.trim() !== '') {
      synth.cancel();
      const fala = new SpeechSynthesisUtterance(texto);
      fala.lang = 'pt-BR';
      synth.speak(fala);
    }
  }

  // Pausar Animações
  toggleAnimacoes.addEventListener('click', () => {
    document.body.classList.toggle('no-animacoes');
  });
});
});