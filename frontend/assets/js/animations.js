// Gerenciador de Animações e Efeitos Visuais
class AnimationManager {
    constructor() {
        this.init();
    }

    init() {
        this.setupMascotAnimations();
        this.setupHoverEffects();
        this.setupLoadingAnimations();
        this.setupParticleEffects();
    }

    // Animações do mascote
    setupMascotAnimations() {
        const mascot = document.getElementById('mascot');
        
        // Animação de clique no mascote
        mascot.addEventListener('click', () => {
            this.mascotCelebration();
        });

        // Animação periódica do mascote
        setInterval(() => {
            this.mascotRandomAnimation();
        }, 10000); // A cada 10 segundos
    }

    // Celebração do mascote
    mascotCelebration() {
        const mascot = document.getElementById('mascot');
        mascot.style.animation = 'none';
        
        setTimeout(() => {
            mascot.style.animation = 'celebration 1s ease-in-out';
        }, 10);

        // Adicionar efeito de partículas
        this.createParticles(mascot, 15);
        
        // Falar algo encorajador se áudio estiver ativo
        if (window.accessibilityManager && window.accessibilityManager.isAudioEnabled) {
            const encouragements = [
                'Você está indo muito bem!',
                'Continue assim!',
                'Que legal!',
                'Você é incrível!',
                'Parabéns!'
            ];
            const randomEncouragement = encouragements[Math.floor(Math.random() * encouragements.length)];
            window.accessibilityManager.speak(randomEncouragement);
        }
    }

    // Animação aleatória do mascote
    mascotRandomAnimation() {
        const mascot = document.getElementById('mascot');
        const animations = ['bounce', 'pulse', 'float'];
        const randomAnimation = animations[Math.floor(Math.random() * animations.length)];
        
        mascot.style.animation = `${randomAnimation} 2s ease-in-out`;
    }

    // Configurar efeitos de hover
    setupHoverEffects() {
        // Efeito de hover para cards de categoria
        document.addEventListener('mouseover', (e) => {
            if (e.target.classList.contains('category-card')) {
                this.cardHoverEffect(e.target, true);
            }
        });

        document.addEventListener('mouseout', (e) => {
            if (e.target.classList.contains('category-card')) {
                this.cardHoverEffect(e.target, false);
            }
        });

        // Efeito de hover para cards de resposta
        document.addEventListener('mouseover', (e) => {
            if (e.target.classList.contains('answer-card')) {
                this.answerCardHoverEffect(e.target, true);
            }
        });

        document.addEventListener('mouseout', (e) => {
            if (e.target.classList.contains('answer-card')) {
                this.answerCardHoverEffect(e.target, false);
            }
        });
    }

    // Efeito de hover para cards
    cardHoverEffect(card, isHovering) {
        if (isHovering) {
            card.style.transform = 'translateY(-10px) scale(1.02)';
            card.style.boxShadow = '0 15px 40px rgba(0, 0, 0, 0.2)';
            
            // Adicionar brilho sutil
            this.addGlowEffect(card);
        } else {
            card.style.transform = 'translateY(0) scale(1)';
            card.style.boxShadow = '0 4px 20px rgba(0, 0, 0, 0.1)';
            
            // Remover brilho
            this.removeGlowEffect(card);
        }
    }

    // Efeito de hover para cards de resposta
    answerCardHoverEffect(card, isHovering) {
        if (window.activityManager && window.activityManager.isAnswered) return;
        
        if (isHovering) {
            card.style.transform = 'translateY(-5px) scale(1.05)';
            card.style.boxShadow = '0 10px 30px rgba(0, 0, 0, 0.2)';
            
            // Efeito de pulso no ícone
            const icon = card.querySelector('i');
            if (icon) {
                icon.style.animation = 'pulse 1s infinite';
            }
        } else {
            card.style.transform = 'translateY(0) scale(1)';
            card.style.boxShadow = '0 4px 20px rgba(0, 0, 0, 0.1)';
            
            // Parar animação do ícone
            const icon = card.querySelector('i');
            if (icon) {
                icon.style.animation = 'none';
            }
        }
    }

    // Adicionar efeito de brilho
    addGlowEffect(element) {
        element.style.position = 'relative';
        
        if (!element.querySelector('.glow-effect')) {
            const glow = document.createElement('div');
            glow.className = 'glow-effect';
            glow.style.cssText = `
                position: absolute;
                top: -2px;
                left: -2px;
                right: -2px;
                bottom: -2px;
                background: linear-gradient(45deg, #4A90E2, #7ED321, #F5A623, #FF6B9D);
                border-radius: inherit;
                z-index: -1;
                opacity: 0.7;
                filter: blur(8px);
                animation: glow-rotate 2s linear infinite;
            `;
            element.appendChild(glow);
        }
    }

    // Remover efeito de brilho
    removeGlowEffect(element) {
        const glow = element.querySelector('.glow-effect');
        if (glow) {
            glow.remove();
        }
    }

    // Configurar animações de carregamento
    setupLoadingAnimations() {
        // Adicionar CSS para animações de carregamento
        const style = document.createElement('style');
        style.textContent = `
            @keyframes glow-rotate {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
            }
            
            @keyframes celebration {
                0%, 100% { transform: rotate(0deg) scale(1); }
                25% { transform: rotate(-15deg) scale(1.1); }
                75% { transform: rotate(15deg) scale(1.1); }
            }
            
            @keyframes float {
                0%, 100% { transform: translateY(0px); }
                50% { transform: translateY(-20px); }
            }
            
            @keyframes sparkle {
                0%, 100% { opacity: 0; transform: scale(0); }
                50% { opacity: 1; transform: scale(1); }
            }
            
            @keyframes rainbow {
                0% { filter: hue-rotate(0deg); }
                100% { filter: hue-rotate(360deg); }
            }
            
            .loading-dots {
                display: inline-block;
            }
            
            .loading-dots::after {
                content: '';
                animation: loading-dots 1.5s infinite;
            }
            
            @keyframes loading-dots {
                0%, 20% { content: ''; }
                40% { content: '.'; }
                60% { content: '..'; }
                80%, 100% { content: '...'; }
            }
        `;
        document.head.appendChild(style);
    }

    // Configurar efeitos de partículas
    setupParticleEffects() {
        // Criar container para partículas se não existir
        if (!document.getElementById('particlesContainer')) {
            const container = document.createElement('div');
            container.id = 'particlesContainer';
            container.style.cssText = `
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                pointer-events: none;
                z-index: 9998;
            `;
            document.body.appendChild(container);
        }
    }

    // Criar partículas
    createParticles(sourceElement, count = 10) {
        const container = document.getElementById('particlesContainer');
        const rect = sourceElement.getBoundingClientRect();
        const centerX = rect.left + rect.width / 2;
        const centerY = rect.top + rect.height / 2;

        for (let i = 0; i < count; i++) {
            const particle = document.createElement('div');
            particle.style.cssText = `
                position: absolute;
                width: 8px;
                height: 8px;
                background: ${this.getRandomColor()};
                border-radius: 50%;
                left: ${centerX}px;
                top: ${centerY}px;
                pointer-events: none;
                animation: particle-explosion 1s ease-out forwards;
                animation-delay: ${i * 0.1}s;
            `;

            // Adicionar animação CSS dinamicamente
            const angle = (360 / count) * i;
            const distance = 50 + Math.random() * 50;
            const endX = centerX + Math.cos(angle * Math.PI / 180) * distance;
            const endY = centerY + Math.sin(angle * Math.PI / 180) * distance;

            particle.style.setProperty('--end-x', `${endX}px`);
            particle.style.setProperty('--end-y', `${endY}px`);

            container.appendChild(particle);

            // Remover partícula após animação
            setTimeout(() => {
                if (particle.parentNode) {
                    particle.parentNode.removeChild(particle);
                }
            }, 1000);
        }

        // Adicionar CSS para animação de partículas se não existir
        if (!document.getElementById('particle-animation-style')) {
            const style = document.createElement('style');
            style.id = 'particle-animation-style';
            style.textContent = `
                @keyframes particle-explosion {
                    0% {
                        transform: translate(0, 0) scale(1);
                        opacity: 1;
                    }
                    100% {
                        transform: translate(calc(var(--end-x) - 50vw), calc(var(--end-y) - 50vh)) scale(0);
                        opacity: 0;
                    }
                }
            `;
            document.head.appendChild(style);
        }
    }

    // Obter cor aleatória
    getRandomColor() {
        const colors = ['#4A90E2', '#7ED321', '#F5A623', '#FF6B9D', '#9013FE', '#00BCD4'];
        return colors[Math.floor(Math.random() * colors.length)];
    }

    // Animação de transição entre telas
    screenTransition(fromScreen, toScreen, direction = 'fade') {
        const from = document.getElementById(fromScreen);
        const to = document.getElementById(toScreen);

        if (direction === 'slide') {
            // Animação de slide
            from.style.animation = 'slideOutLeft 0.5s ease-in-out forwards';
            
            setTimeout(() => {
                from.classList.add('hidden');
                to.classList.remove('hidden');
                to.style.animation = 'slideInRight 0.5s ease-in-out forwards';
            }, 250);
        } else {
            // Animação de fade (padrão)
            from.style.animation = 'fadeOut 0.3s ease-in-out forwards';
            
            setTimeout(() => {
                from.classList.add('hidden');
                to.classList.remove('hidden');
                to.style.animation = 'fadeIn 0.3s ease-in-out forwards';
            }, 150);
        }

        // Adicionar CSS para animações de transição se não existir
        if (!document.getElementById('transition-animations-style')) {
            const style = document.createElement('style');
            style.id = 'transition-animations-style';
            style.textContent = `
                @keyframes slideOutLeft {
                    from { transform: translateX(0); opacity: 1; }
                    to { transform: translateX(-100%); opacity: 0; }
                }
                
                @keyframes slideInRight {
                    from { transform: translateX(100%); opacity: 0; }
                    to { transform: translateX(0); opacity: 1; }
                }
                
                @keyframes fadeOut {
                    from { opacity: 1; }
                    to { opacity: 0; }
                }
                
                @keyframes fadeIn {
                    from { opacity: 0; }
                    to { opacity: 1; }
                }
            `;
            document.head.appendChild(style);
        }
    }

    // Efeito de digitação para texto
    typewriterEffect(element, text, speed = 50) {
        element.textContent = '';
        let i = 0;
        
        const timer = setInterval(() => {
            if (i < text.length) {
                element.textContent += text.charAt(i);
                i++;
            } else {
                clearInterval(timer);
            }
        }, speed);
    }

    // Efeito de contagem animada
    animateCounter(element, start, end, duration = 1000) {
        const range = end - start;
        const increment = range / (duration / 16); // 60 FPS
        let current = start;
        
        const timer = setInterval(() => {
            current += increment;
            if (current >= end) {
                current = end;
                clearInterval(timer);
            }
            element.textContent = Math.floor(current);
        }, 16);
    }

    // Efeito de shake para elementos
    shakeElement(element, intensity = 5, duration = 600) {
        const originalTransform = element.style.transform;
        let start = null;
        
        const shake = (timestamp) => {
            if (!start) start = timestamp;
            const progress = timestamp - start;
            
            if (progress < duration) {
                const x = (Math.random() - 0.5) * intensity;
                const y = (Math.random() - 0.5) * intensity;
                element.style.transform = `${originalTransform} translate(${x}px, ${y}px)`;
                requestAnimationFrame(shake);
            } else {
                element.style.transform = originalTransform;
            }
        };
        
        requestAnimationFrame(shake);
    }

    // Efeito de rainbow para elementos especiais
    rainbowEffect(element, duration = 3000) {
        element.style.animation = `rainbow ${duration}ms linear infinite`;
        
        setTimeout(() => {
            element.style.animation = '';
        }, duration);
    }

    // Criar efeito de estrelas cadentes
    createShootingStars(count = 5) {
        const container = document.getElementById('particlesContainer');
        
        for (let i = 0; i < count; i++) {
            const star = document.createElement('div');
            star.style.cssText = `
                position: absolute;
                width: 2px;
                height: 2px;
                background: white;
                border-radius: 50%;
                box-shadow: 0 0 10px white;
                top: ${Math.random() * 50}%;
                left: -10px;
                animation: shooting-star 2s linear forwards;
                animation-delay: ${i * 0.5}s;
            `;
            
            container.appendChild(star);
            
            setTimeout(() => {
                if (star.parentNode) {
                    star.parentNode.removeChild(star);
                }
            }, 2500);
        }
        
        // Adicionar CSS para estrelas cadentes se não existir
        if (!document.getElementById('shooting-star-style')) {
            const style = document.createElement('style');
            style.id = 'shooting-star-style';
            style.textContent = `
                @keyframes shooting-star {
                    0% {
                        transform: translateX(0) translateY(0);
                        opacity: 1;
                    }
                    100% {
                        transform: translateX(100vw) translateY(50vh);
                        opacity: 0;
                    }
                }
            `;
            document.head.appendChild(style);
        }
    }
}

// Inicializar gerenciador de animações quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', () => {
    window.animationManager = new AnimationManager();
});

