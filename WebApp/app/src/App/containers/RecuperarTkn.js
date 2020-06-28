import React from 'react';
import axios from 'axios';
import {
    Link,
    withRouter
} from "react-router-dom";
import './RecuperarPass.css';

class RecuperarPass extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            password: '',
            passwordconfirm: '',
            token: '',
            warning: false,
            success: false,
            failure: false
        };

        this.handleRecuperarButton = this.handleRecuperarButton.bind(this);
        this.handleChangeTkn = this.handleChangeTkn.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangePasswordConfirm = this.handleChangePasswordConfirm.bind(this);
    }

    handleChangeTkn(event) {
        this.setState({ token: event.target.value });
    }
    handleChangePassword(event) {
        this.setState({ password: event.target.value });
    }
    handleChangePasswordConfirm(event) {
        this.setState({ passwordconfirm: event.target.value });
    }

    handleRecuperarButton(event) {
        event.preventDefault();

        if (this.state.password.length === 0 || this.state.token.length === 0 || this.state.password != this.state.passwordconfirm) {
            this.setState({ warning: true });
        } else {
            this.setState({warning: false});
            axios({
                method: 'put',
                url: 'https://localhost:44301/' + this.props.accounttype + '/Verificatoken',
                data: JSON.stringify(this.props.username + '-|-' + this.state.token + '-|-' + this.state.password),
                headers: {
                    "Content-Type": "application/json",
                }
            })
                .then(
                    this.setState({ failure: false, success: true, warning: false })
                )
                .catch(
                    this.setState({ success: false, warning: false, failure: true })
                )
        }
    }

    render() {
        return (
            <div className="container login-container">
                <div className="row">
                    <div className="col"></div>
                    <div className="col-md-7">
                        <div className="card loginOnly-card">
                            <div className="card-block" >
                                <h3 style={{ marginBottom: '20px' }} className="card-title login-title">Gestão de Florestas</h3>
                                <p style={{ marginBottom: '40px' }} class="text-secondary">Verifique o seu email e utilize o código para alteração de password.</p>
                                <form className="login-form">
                                    <div className="form-group">
                                        <input type="text" value={this.state.recuptkn} className="form-control" id="codeInput" onChange={this.handleChangeTkn} placeholder="Código"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="password" value={this.state.password} className="form-control" id="passwordInput" onChange={this.handleChangePassword} placeholder="Nova Password"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="password" value={this.state.passwordconfirm} className="form-control" id="passwordConfirmInput" onChange={this.handleChangePasswordConfirm} placeholder="Confirme Password"></input>
                                    </div>
                                    <div className="form-group">
                                        <p>{this.state.warning ? 'Verifique que as passwords são iguais, e que o código está correto.' : ''}</p>
                                        <p>{this.state.failure ? 'Código incorreto. Verifique o código e tente novamente.' : ''}</p>
                                        <p>{this.state.success ? 'Password alterada com successo.' : ''}</p>
                                    </div>
                                    <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleRecuperarButton} value="Recuperar" />
                                    <Link to="/login"><input className="btn login-btn btn-success btn-sm" type='button' value="Voltar" /></Link>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="col"></div>
                </div>
            </div>
        );
    }
}

export default withRouter(RecuperarPass);