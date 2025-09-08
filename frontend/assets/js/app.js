// Aplicação Principal - Mentimeter Kids
class MentimeterKidsApp {
    constructor() {
        this.currentScreen = 'welcomeScreen';
        this.init();
    }

    init() {
        // Aguardar carregamento completo do DOM
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setupApp());
        } else {
            this.setupApp();
        }
    }

    setupApp() {
        console.log('🎮 Iniciando Mentimeter Kids...');
        
        // Configurar event listeners
        this.setupEventListeners();
        
        // Configurar service worker para cache (opcional)
        this.setupServiceWorker();
        
        // Mostrar tela de boas-vindas
        this.showWelcomeScreen();
        
        // Configurar detecção de dispositivo
        this.setupDeviceDetection();
        
        console.log('✅ Mentimeter Kids iniciado com sucesso!');
    }

    setupEventListeners() {
        // Event listeners para categorias
        document.querySelectorAll('.category-card').forEach(card => {
            card.addEventListener('click', (e) => {
                const category = card.dataset.category;
                this.startActivity(category);
            });
            
            // Adicionar suporte a teclado
            card.addEventListener('keydown', (e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    const category = card.dataset.category;
                    this.startActivity(category);
                }
            });
            
            // Tornar focável
            card.setAttribute('tabindex', '0');
        });

        // Event listeners para botões de controle
        document.getElementById('nextBtn').addEventListener('click', () => {
            window.activityManager.nextQuestion();
        });

        document.getElementById('continueBtn').addEventListener('click', () => {
            window.activityManager.continueFeedback();
        });

        document.getElementById('helpBtn').addEventListener('click', () => {
            this.showHelp();
        });

        document.getElementById('playAgainBtn').addEventListener('click', () => {
            window.activityManager.playAgain();
        });

        document.getElementById('newCategoryBtn').addEventListener('click', () => {
            this.showWelcomeScreen();
        });

        // Event listener para mascote
        document.getElementById('mascot').addEventListener('click', () => {
            this.mascotInteraction();
        });

        // Event listeners para gestos touch
        this.setupTouchGestures();
        
        // Event listener para redimensionamento da janela
        window.addEventListener('resize', () => {
            this.handleResize();
        });

        // Event listener para mudança de orientação
        window.addEventListener('orientationchange', () => {
            setTimeout(() => this.handleOrientationChange(), 100);
        });

        // Event listener para visibilidade da página
        document.addEventListener('visibilitychange', () => {
            this.handleVisibilityChange();
        });
    }

    // Iniciar atividade de uma categoria
    startActivity(category) {
        console.log(`🎯 Iniciando atividade: ${category}`);
        
        // Feedback visual
        const categoryCard = document.querySelector(`[data-category="${category}"]`);
        if (categoryCard) {
            categoryCard.style.transform = 'scale(0.95)';
            setTimeout(() => {
                categoryCard.style.transform = '';
            }, 150);
        }

        // Anunciar para acessibilidade
        if (window.accessibilityManager) {
            const categoryNames = {
                matematica: 'Matemática',
                portugues: 'Português',
                ciencias: 'Ciências',
                geografia: 'Geografia'
            };
            window.accessibilityManager.announce(`Iniciando atividades de ${categoryNames[category]}`);
        }

        // Transição para tela de atividade
        setTimeout(() => {
            window.activityManager.startCategory(category);
            this.showActivityScreen();
            
            // Ler pergunta se áudio estiver ativo
            setTimeout(() => {
                if (window.accessibilityManager && window.accessibilityManager.isAudioEnabled) {
                    window.accessibilityManager.readCurrentQuestion();
                }
            }, 500);
        }, 200);
    }

    // Mostrar tela de boas-vindas
    showWelcomeScreen() {
        this.showScreen('welcomeScreen');
        
        // Animar entrada das categorias
        setTimeout(() => {
            document.querySelectorAll('.category-card').forEach((card, index) => {
                card.style.opacity = '0';
                card.style.transform = 'translateY(20px)';
                
                setTimeout(() => {
                    card.style.transition = 'all 0.5s ease';
                    card.style.opacity = '1';
                    card.style.transform = 'translateY(0)';
                }, index * 100);
            });
        }, 100);

        // Falar boas-vindas se áudio estiver ativo
        if (window.accessibilityManager && window.accessibilityManager.isAudioEnabled) {
            setTimeout(() => {
                window.accessibilityManager.speak('Bem-vindos às Atividades Divertidas! Escolha uma categoria para começar a aprender brincando.');
            }, 1000);
        }
    }

    // Mostrar tela de atividade
    showActivityScreen() {
        this.showScreen('activityScreen');
        
        // Animar entrada da pergunta
        const questionCard = document.querySelector('.question-card');
        if (questionCard) {
            questionCard.style.opacity = '0';
            questionCard.style.transform = 'scale(0.9)';
            
            setTimeout(() => {
                questionCard.style.transition = 'all 0.5s ease';
                questionCard.style.opacity = '1';
                questionCard.style.transform = 'scale(1)';
            }, 100);
        }

        // Animar entrada das respostas
        setTimeout(() => {
            document.querySelectorAll('.answer-card').forEach((card, index) => {
                card.style.opacity = '0';
                card.style.transform = 'translateY(20px)';
                
                setTimeout(() => {
                    card.style.transition = 'all 0.3s ease';
                    card.style.opacity = '1';
                    card.style.transform = 'translateY(0)';
                }, index * 100);
            });
        }, 300);
    }

    // Mostrar tela específica
    showScreen(screenId) {
        // Esconder todas as telas
        document.querySelectorAll('.screen').forEach(screen => {
            screen.classList.add('hidden');
        });
        
        // Mostrar tela solicitada
        const targetScreen = document.getElementById(screenId);
        if (targetScreen) {
            targetScreen.classList.remove('hidden');
            this.currentScreen = screenId;
            
            // Anunciar mudança de tela para acessibilidade
            if (window.accessibilityManager) {
                const screenNames = {
                    welcomeScreen: 'Tela de boas-vindas',
                    activityScreen: 'Tela de atividade',
                    feedbackScreen: 'Tela de feedback',
                    resultsScreen: 'Tela de resultados'
                };
                window.accessibilityManager.announce(screenNames[screenId] || 'Nova tela');
            }
        }
    }

    // Interação com mascote
    mascotInteraction() {
        const mascot = document.getElementById('mascot');
        
        // Animação especial
        if (window.animationManager) {
            window.animationManager.mascotCelebration();
        }
        
        // Mensagens aleatórias do mascote
        const messages = [
            'Olá! Eu sou seu amigo urso! 🐻',
            'Você está indo muito bem!',
            'Que tal uma nova atividade?',
            'Aprender é divertido!',
            'Continue assim, você é incrível!'
        ];
        
        const randomMessage = messages[Math.floor(Math.random() * messages.length)];
        
        // Mostrar mensagem
        this.showToast(randomMessage);
        
        // Falar mensagem se áudio estiver ativo
        if (window.accessibilityManager && window.accessibilityManager.isAudioEnabled) {
            window.accessibilityManager.speak(randomMessage);
        }
    }

    // Mostrar ajuda
    showHelp() {
        const helpContent = `
            <div style="text-align: left; max-width: 400px;">
                <h3>Como jogar:</h3>
                <ul>
                    <li>📱 Toque nas opções para responder</li>
                    <li>🎵 Use o botão de áudio para ouvir as perguntas</li>
                    <li>🔍 Use o botão de contraste para melhor visualização</li>
                    <li>📝 Use o botão de fonte para texto maior</li>
                    <li>⌨️ Use números 1-4 no teclado para responder</li>
                    <li>🐻 Clique no ursinho para interagir</li>
                </ul>
                <p><strong>Dica:</strong> Ganhe pontos respondendo corretamente!</p>
            </div>
        `;
        
        this.showModal('Ajuda', helpContent);
    }

    // Mostrar modal
    showModal(title, content) {
        // Criar modal se não existir
        let modal = document.getElementById('helpModal');
        if (!modal) {
            modal = document.createElement('div');
            modal.id = 'helpModal';
            modal.innerHTML = `
                <div class="modal-overlay">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h2 class="modal-title"></h2>
                            <button class="modal-close" aria-label="Fechar">&times;</button>
                        </div>
                        <div class="modal-body"></div>
                    </div>
                </div>
            `;
            
            // Adicionar estilos do modal
            const modalStyles = `
                .modal-overlay {
                    position: fixed;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    background: rgba(0, 0, 0, 0.5);
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    z-index: 10000;
                    animation: fadeIn 0.3s ease;
                }
                
                .modal-content {
                    background: white;
                    border-radius: 20px;
                    max-width: 90%;
                    max-height: 90%;
                    overflow-y: auto;
                    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
                    animation: slideIn 0.3s ease;
                }
                
                .modal-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    padding: 20px;
                    border-bottom: 1px solid #eee;
                }
                
                .modal-title {
                    margin: 0;
                    color: #2C3E50;
                    font-size: 1.5rem;
                }
                
                .modal-close {
                    background: none;
                    border: none;
                    font-size: 24px;
                    cursor: pointer;
                    color: #666;
                    width: 30px;
                    height: 30px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    border-radius: 50%;
                    transition: all 0.3s ease;
                }
                
                .modal-close:hover {
                    background: #f0f0f0;
                    color: #333;
                }
                
                .modal-body {
                    padding: 20px;
                }
                
                @keyframes slideIn {
                    from { transform: translateY(-50px); opacity: 0; }
                    to { transform: translateY(0); opacity: 1; }
                }
            `;
            
            // Adicionar estilos se não existirem
            if (!document.getElementById('modal-styles')) {
                const style = document.createElement('style');
                style.id = 'modal-styles';
                style.textContent = modalStyles;
                document.head.appendChild(style);
            }
            
            document.body.appendChild(modal);
            
            // Event listeners do modal
            modal.querySelector('.modal-close').addEventListener('click', () => {
                this.closeModal();
            });
            
            modal.querySelector('.modal-overlay').addEventListener('click', (e) => {
                if (e.target === modal.querySelector('.modal-overlay')) {
                    this.closeModal();
                }
            });
        }
        
        // Atualizar conteúdo
        modal.querySelector('.modal-title').textContent = title;
        modal.querySelector('.modal-body').innerHTML = content;
        
        // Mostrar modal
        modal.style.display = 'block';
        
        // Focar no botão de fechar para acessibilidade
        setTimeout(() => {
            modal.querySelector('.modal-close').focus();
        }, 100);
    }

    // Fechar modal
    closeModal() {
        const modal = document.getElementById('helpModal');
        if (modal) {
            modal.style.display = 'none';
        }
    }

    // Mostrar toast/notificação
    showToast(message, duration = 3000) {
        // Criar toast se não existir
        let toast = document.getElementById('toast');
        if (!toast) {
            toast = document.createElement('div');
            toast.id = 'toast';
            toast.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: #4A90E2;
                color: white;
                padding: 15px 20px;
                border-radius: 10px;
                box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
                z-index: 9999;
                transform: translateX(100%);
                transition: transform 0.3s ease;
                max-width: 300px;
                font-weight: 500;
            `;
            document.body.appendChild(toast);
        }
        
        // Atualizar mensagem
        toast.textContent = message;
        
        // Mostrar toast
        toast.style.transform = 'translateX(0)';
        
        // Esconder após duração especificada
        setTimeout(() => {
            toast.style.transform = 'translateX(100%)';
        }, duration);
    }

    // Configurar gestos touch
    setupTouchGestures() {
        let touchStartX = 0;
        let touchStartY = 0;
        let touchStartTime = 0;
        
        document.addEventListener('touchstart', (e) => {
            touchStartX = e.touches[0].clientX;
            touchStartY = e.touches[0].clientY;
            touchStartTime = Date.now();
        }, { passive: true });
        
        document.addEventListener('touchend', (e) => {
            const touchEndX = e.changedTouches[0].clientX;
            const touchEndY = e.changedTouches[0].clientY;
            const touchEndTime = Date.now();
            
            const deltaX = touchEndX - touchStartX;
            const deltaY = touchEndY - touchStartY;
            const deltaTime = touchEndTime - touchStartTime;
            
            // Verificar se é um swipe (movimento rápido)
            if (deltaTime < 300 && Math.abs(deltaX) > 50) {
                if (deltaX > 0) {
                    // Swipe para direita - voltar
                    this.handleSwipeRight();
                } else {
                    // Swipe para esquerda - avançar
                    this.handleSwipeLeft();
                }
            }
            
            // Swipe para baixo - repetir áudio
            if (deltaTime < 300 && deltaY > 50 && Math.abs(deltaX) < 50) {
                this.handleSwipeDown();
            }
        }, { passive: true });
    }

    // Lidar com swipe para direita
    handleSwipeRight() {
        if (this.currentScreen === 'activityScreen') {
            // Voltar para categorias
            this.showWelcomeScreen();
        }
    }

    // Lidar com swipe para esquerda
    handleSwipeLeft() {
        if (this.currentScreen === 'activityScreen') {
            const nextBtn = document.getElementById('nextBtn');
            if (!nextBtn.classList.contains('hidden')) {
                nextBtn.click();
            }
        }
    }

    // Lidar com swipe para baixo
    handleSwipeDown() {
        if (window.accessibilityManager && window.accessibilityManager.isAudioEnabled) {
            if (this.currentScreen === 'activityScreen') {
                window.accessibilityManager.readCurrentQuestion();
            }
        }
    }

    // Configurar detecção de dispositivo
    setupDeviceDetection() {
        // Detectar tipo de dispositivo
        const isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        const isTablet = /(tablet|ipad|playbook|silk)|(android(?!.*mobi))/i.test(navigator.userAgent);
        
        if (isMobile) {
            document.body.classList.add('mobile-device');
        }
        
        if (isTablet) {
            document.body.classList.add('tablet-device');
        }
        
        // Detectar capacidades do dispositivo
        if ('vibrate' in navigator) {
            document.body.classList.add('vibration-support');
        }
        
        if ('serviceWorker' in navigator) {
            document.body.classList.add('pwa-support');
        }
    }

    // Lidar com redimensionamento
    handleResize() {
        // Ajustar layout para diferentes tamanhos de tela
        const width = window.innerWidth;
        
        if (width < 768) {
            document.body.classList.add('small-screen');
        } else {
            document.body.classList.remove('small-screen');
        }
    }

    // Lidar com mudança de orientação
    handleOrientationChange() {
        // Forçar recálculo do layout
        document.body.style.height = window.innerHeight + 'px';
        
        // Mostrar dica de orientação se necessário
        if (window.innerWidth < window.innerHeight && window.innerWidth < 768) {
            this.showToast('💡 Dica: Gire o dispositivo para uma melhor experiência!', 4000);
        }
    }

    // Lidar com mudança de visibilidade
    handleVisibilityChange() {
        if (document.hidden) {
            // Página ficou oculta - pausar áudio
            if (window.speechSynthesis) {
                window.speechSynthesis.pause();
            }
        } else {
            // Página ficou visível - retomar áudio
            if (window.speechSynthesis) {
                window.speechSynthesis.resume();
            }
        }
    }

    // Configurar service worker
    setupServiceWorker() {
        if ('serviceWorker' in navigator) {
            navigator.serviceWorker.register('/sw.js')
                .then(registration => {
                    console.log('✅ Service Worker registrado:', registration);
                })
                .catch(error => {
                    console.log('❌ Falha ao registrar Service Worker:', error);
                });
        }
    }

    // Método para vibração (feedback tátil)
    vibrate(pattern = [100]) {
        if ('vibrate' in navigator) {
            navigator.vibrate(pattern);
        }
    }

    // Salvar progresso localmente
    saveProgress() {
        const progress = {
            currentCategory: window.activityManager.currentCategory,
            score: window.activityManager.score,
            correctAnswers: window.activityManager.correctAnswers,
            timestamp: Date.now()
        };
        
        localStorage.setItem('mentimeterKidsProgress', JSON.stringify(progress));
    }

    // Carregar progresso salvo
    loadProgress() {
        const saved = localStorage.getItem('mentimeterKidsProgress');
        if (saved) {
            try {
                const progress = JSON.parse(saved);
                // Verificar se o progresso é recente (menos de 1 hora)
                if (Date.now() - progress.timestamp < 3600000) {
                    return progress;
                }
            } catch (e) {
                console.log('Erro ao carregar progresso:', e);
            }
        }
        return null;
    }
}

// Inicializar aplicação
window.addEventListener('load', () => {
    window.mentimeterKidsApp = new MentimeterKidsApp();
});

// Prevenir zoom em dispositivos móveis
document.addEventListener('gesturestart', function (e) {
    e.preventDefault();
});

// Prevenir seleção de texto em dispositivos touch
document.addEventListener('selectstart', function (e) {
    if (e.target.tagName !== 'INPUT' && e.target.tagName !== 'TEXTAREA') {
        e.preventDefault();
    }
});

// Adicionar meta tag para PWA se não existir
if (!document.querySelector('meta[name="theme-color"]')) {
    const themeColor = document.createElement('meta');
    themeColor.name = 'theme-color';
    themeColor.content = '#4A90E2';
    document.head.appendChild(themeColor);
}

