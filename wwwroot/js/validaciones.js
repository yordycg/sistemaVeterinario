class Validaciones {
    static soloNumeros(evt) {
        if (evt.keyCode >= 48 && evt.keyCode <= 57) {
            return true;
        }
        return false;
    }

    static validarEmail(email) {
        const formato = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return formato.test(email);
    }

    static limpiar() {
        document.querySelectorAll('.form-control, .form-select').forEach(element => {
            element.classList.remove('is-invalid');
            element.classList.remove('is-valid');

            const errorSpan = document.getElementById(`e-${element.id}`)
            if (errorSpan) {
                errorSpan.innerText = '';
            }
            element.value = '';
        })
    }

    static validarRun(run) {
        const Fn = {
            validaRut: function (rutCompleto) {
                rutCompleto = rutCompleto.replace("‐", "-");

                if (!/^[0-9]+[-|-]{1}[0-9kK]{1}$/.test(rutCompleto)) {
                    return false;
                }

                let tmp = rutCompleto.split('-');
                let digv = tmp[1];
                let rut = tmp[0];

                if (digv === 'K') {
                    digv = 'k';
                }

                return (Fn.dv(rut) === digv);
            },
            dv: function (T) {
                let M = 0, S = 1;
                for (; T; T = Math.floor(T / 10)) {
                    S = (S + T % 10 * (9 - M++ % 6)) % 11;
                }
                return S ? S - 1 : 'k';
            }
        };

        return Fn.validaRut(run);
    }

}