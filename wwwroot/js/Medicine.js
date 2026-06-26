const urlRadio = document.getElementById('modeUrl');
const fileRadio = document.getElementById('modeFile');
const urlField = document.getElementById('urlForm');
const fileField = document.getElementById('fileForm');

function toggleForms() {
  if (urlRadio.checked) {
    urlField.style.display = 'block';
    fileField.style.display = 'none';
  } else {
    urlField.style.display = 'none';
    fileField.style.display = 'block';
  }
}
// alert(`urlRadio: ${urlRadio}, fileRadio: ${fileRadio}, urlField: ${urlField}, fileField: ${fileField}`);
urlRadio.addEventListener('change', toggleForms);
fileRadio.addEventListener('change', toggleForms);
