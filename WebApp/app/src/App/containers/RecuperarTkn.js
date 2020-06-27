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
            warning: false,
        };

        this.handleRecuperarButton = this.handleRecuperarButton.bind(this);
    }


    handleRecuperarButton(event) {
        event.preventDefault();

        if(this.state.password.length === 0 || this.state.password != this.state.passwordconfirm)
        {
            this.setState({warning: true});
        } else {
            //axios request change
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
                                        <p>{this.state.warning ? 'Verifique que as passwords são iguas e o código está correto.' : ''}</p>
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