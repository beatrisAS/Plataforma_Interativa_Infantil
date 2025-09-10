function increaseFont(){ document.body.style.fontSize = (parseFloat(getComputedStyle(document.body).fontSize) + 2) + 'px'; }
function decreaseFont(){ document.body.style.fontSize = (parseFloat(getComputedStyle(document.body).fontSize) - 2) + 'px'; }
document.getElementById && document.addEventListener('click', function(e){ if(e.target && e.target.id==='themeToggle'){ document.body.classList.toggle('dark'); document.body.classList.toggle('light'); } });
