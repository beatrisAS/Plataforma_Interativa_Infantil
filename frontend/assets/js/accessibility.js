// Gerenciador de Acessibilidade
class AccessibilityManager {
    constructor() {
        this.isAudioEnabled = false;
        this.isHighContrast = false;
        this.isLargeFont = false;
        this.speechSynthesis = window.speechSynthesis;
        this.currentVoice = null;
        
        this.init();
    }

    init() {
        // Configurar vozes disponíveis
        this.setupVoices();
        
        // Event listeners para botões de acessibilidade
        document.getElementById('audioBtn').addEventListener('click', () => this.toggleAudio());
        document.getElementById('contrastBtn').addEventListener('click', () => this.toggleHighContrast());
        document.getElementById('fontSizeBtn').addEventListener('click', () => this.toggleLargeFont());
        
        // Carregar preferências salvas
        this.loadPreferences();
        
        // Adicionar suporte a teclado
        this.setupKeyboardNavigation();
        
        // Adicionar ARIA labels
        this.setupAriaLabels();
    }

    // Configurar vozes para síntese de fala
    setupVoices() {
        const setVoice = () => {
            const voices = this.speechSynthesis.getVoices();
            // Procurar por voz em português brasileiro
            this.currentVoice = voices.find(voice => 
                voice.lang.includes('pt-BR') || voice.lang.includes('pt')
            ) || voices[0];
        };

        setVoice();
        this.speechSynthesis.onvoiceschanged = setVoice;
    }

    // Toggle áudio/narração
    toggleAudio() {
        this.isAudioEnabled = !this.isAudioEnabled;
        const audioBtn = document.getElementById('audioBtn');
        
        if (this.isAudioEnabled) {
            audioBtn.classList.add('active');
            audioBtn.title = 'Desativar Áudio';
            this.speak('Áudio ativado. Agora eu vou ler as perguntas e opções para você.');
        } else {
            audioBtn.classList.remove('active');
            audioBtn.title = 'Ativar Áudio';
            this.speechSynthesis.cancel();
        }
        
        this.savePreferences();
    }

    // Toggle alto contraste
    toggleHighContrast() {
        this.isHighContrast = !this.isHighContrast;
        const contrastBtn = document.getElementById('contrastBtn');
        
        if (this.isHighContrast) {
            document.body.classList.add('high-contrast');
            contrastBtn.classList.add('active');
            contrastBtn.title = 'Desativar Alto Contraste';
            this.speak('Alto contraste ativado.');
        } else {
            document.body.classList.remove('high-contrast');
            contrastBtn.classList.remove('active');
            contrastBtn.title = 'Ativar Alto Contraste';
        }
        
        this.savePreferences();
    }

    // Toggle fonte grande
    toggleLargeFont() {
        this.isLargeFont = !this.isLargeFont;
        const fontBtn = document.getElementById('fontSizeBtn');
        
        if (this.isLargeFont) {
            document.body.classList.add('large-font');
            fontBtn.classList.add('active');
            fontBtn.title = 'Desativar Fonte Grande';
            this.speak('Fonte grande ativada.');
        } else {
            document.body.classList.remove('large-font');
            fontBtn.classList.remove('active');
            fontBtn.title = 'Ativar Fonte Grande';
        }
        
        this.savePreferences();
    }

    // Síntese de fala
    speak(text, priority = false) {
        if (!this.isAudioEnabled && !priority) return;
        
        // Cancelar fala anterior se for prioridade
        if (priority) {
            this.speechSynthesis.cancel();
        }
        
        const utterance = new SpeechSynthesisUtterance(text);
        utterance.voice = this.currentVoice;
        utterance.rate = 0.8; // Velocidade mais lenta para crianças
        utterance.pitch = 1.1; // Tom ligeiramente mais alto
        utterance.volume = 0.8;
        
        this.speechSynthesis.speak(utterance);
    }

    // Ler pergunta atual
    readCurrentQuestion() {
        if (!this.isAudioEnabled) return;
        
        const questionText = document.getElementById('questionText').textContent;
        this.speak(`Pergunta: ${questionText}`);
        
        // Ler opções após um delay
        setTimeout(() => {
            this.readAnswerOptions();
        }, 2000);
    }

    // Ler opções de resposta
    readAnswerOptions() {
        if (!this.isAudioEnabled) return;
        
        const answerCards = document.querySelectorAll('.answer-card');
        let optionsText = 'As opções são: ';
        
        answerCards.forEach((card, index) => {
            const answerText = card.querySelector('.answer-text').textContent;
            optionsText += `Opção ${index + 1}: ${answerText}. `;
        });
        
        this.speak(optionsText);
    }

    // Ler feedback
    readFeedback(isCorrect, explanation) {
        if (!this.isAudioEnabled) return;
        
        const feedbackText = isCorrect ? 
            `Parabéns! Resposta correta. ${explanation}` :
            `Não foi dessa vez. ${explanation} Continue tentando, você vai conseguir!`;
            
        this.speak(feedbackText, true);
    }

    // Configurar navegação por teclado
    setupKeyboardNavigation() {
        document.addEventListener('keydown', (e) => {
            switch(e.key) {
                case '1':
                case '2':
                case '3':
                case '4':
                    this.selectAnswerByNumber(parseInt(e.key) - 1);
                    break;
                case 'Enter':
                case ' ':
                    this.handleEnterKey(e);
                    break;
                case 'h':
                case 'H':
                    this.showHelp();
                    break;
                case 'r':
                case 'R':
                    if (this.isAudioEnabled) {
                        this.readCurrentQuestion();
                    }
                    break;
                case 'Escape':
                    this.speechSynthesis.cancel();
                    break;
            }
        });
    }

    // Selecionar resposta por número
    selectAnswerByNumber(index) {
        const answerCards = document.querySelectorAll('.answer-card');
        if (answerCards[index] && !window.activityManager.isAnswered) {
            answerCards[index].click();
        }
    }

    // Lidar com tecla Enter
    handleEnterKey(e) {
        e.preventDefault();
        
        // Verificar qual botão está visível e ativo
        const nextBtn = document.getElementById('nextBtn');
        const continueBtn = document.getElementById('continueBtn');
        const playAgainBtn = document.getElementById('playAgainBtn');
        
        if (!nextBtn.classList.contains('hidden')) {
            nextBtn.click();
        } else if (!continueBtn.classList.contains('hidden')) {
            continueBtn.click();
        } else if (!playAgainBtn.classList.contains('hidden')) {
            playAgainBtn.click();
        }
    }

    // Mostrar ajuda
    showHelp() {
        const helpText = `
            Comandos de teclado disponíveis:
            - Números 1, 2, 3, 4: Selecionar resposta
            - Enter ou Espaço: Continuar/Próxima
            - H: Mostrar esta ajuda
            - R: Repetir pergunta (se áudio estiver ativo)
            - Escape: Parar narração
        `;
        
        alert(helpText);
        this.speak(helpText, true);
    }

    // Configurar ARIA labels
    setupAriaLabels() {
        // Adicionar labels aos botões de acessibilidade
        document.getElementById('audioBtn').setAttribute('aria-label', 'Ativar ou desativar narração de áudio');
        document.getElementById('contrastBtn').setAttribute('aria-label', 'Ativar ou desativar alto contraste');
        document.getElementById('fontSizeBtn').setAttribute('aria-label', 'Ativar ou desativar fonte grande');
        
        // Adicionar role e labels às áreas principais
        document.querySelector('.main-content').setAttribute('role', 'main');
        document.querySelector('.header-container').setAttribute('role', 'banner');
        
        // Adicionar live region para anúncios
        const liveRegion = document.createElement('div');
        liveRegion.setAttribute('aria-live', 'polite');
        liveRegion.setAttribute('aria-atomic', 'true');
        liveRegion.className = 'sr-only';
        liveRegion.id = 'liveRegion';
        document.body.appendChild(liveRegion);
    }

    // Anunciar mudanças para leitores de tela
    announce(message) {
        const liveRegion = document.getElementById('liveRegion');
        if (liveRegion) {
            liveRegion.textContent = message;
        }
    }

    // Salvar preferências no localStorage
    savePreferences() {
        const preferences = {
            isAudioEnabled: this.isAudioEnabled,
            isHighContrast: this.isHighContrast,
            isLargeFont: this.isLargeFont
        };
        
        localStorage.setItem('accessibilityPreferences', JSON.stringify(preferences));
    }

    // Carregar preferências do localStorage
    loadPreferences() {
        const saved = localStorage.getItem('accessibilityPreferences');
        if (saved) {
            const preferences = JSON.parse(saved);
            
            if (preferences.isAudioEnabled) {
                this.toggleAudio();
            }
            if (preferences.isHighContrast) {
                this.toggleHighContrast();
            }
            if (preferences.isLargeFont) {
                this.toggleLargeFont();
            }
        }
    }

    // Configurar foco para elementos interativos
    setupFocusManagement() {
        // Adicionar indicadores visuais de foco
        const style = document.createElement('style');
        style.textContent = `
            .answer-card:focus,
            .category-card:focus,
            button:focus {
                outline: 3px solid #F5A623;
                outline-offset: 2px;
            }
            
            .sr-only {
                position: absolute;
                width: 1px;
                height: 1px;
                padding: 0;
                margin: -1px;
                overflow: hidden;
                clip: rect(0, 0, 0, 0);
                white-space: nowrap;
                border: 0;
            }
        `;
        document.head.appendChild(style);
    }

    // Adicionar suporte a gestos para dispositivos touch
    setupTouchGestures() {
        let touchStartX = 0;
        let touchStartY = 0;
        
        document.addEventListener('touchstart', (e) => {
            touchStartX = e.touches[0].clientX;
            touchStartY = e.touches[0].clientY;
        });
        
        document.addEventListener('touchend', (e) => {
            const touchEndX = e.changedTouches[0].clientX;
            const touchEndY = e.changedTouches[0].clientY;
            
            const deltaX = touchEndX - touchStartX;
            const deltaY = touchEndY - touchStartY;
            
            // Swipe para a direita - próxima pergunta
            if (deltaX > 50 && Math.abs(deltaY) < 50) {
                const nextBtn = document.getElementById('nextBtn');
                if (!nextBtn.classList.contains('hidden')) {
                    nextBtn.click();
                }
            }
            
            // Swipe para baixo - repetir pergunta
            if (deltaY > 50 && Math.abs(deltaX) < 50) {
                if (this.isAudioEnabled) {
                    this.readCurrentQuestion();
                }
            }
        });
    }
}

// Inicializar gerenciador de acessibilidade quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', () => {
    window.accessibilityManager = new AccessibilityManager();
});

