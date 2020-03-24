import React from 'react';
import axios from 'axios';
import './Register.css'

class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            nome: '',
            nif: '',
            email: '',
            password: '',
            passwordconfirm: '',
            accounttype: 'proprietario'
        };

        this.handleChangeName = this.handleChangeName.bind(this);
        this.handleChangeNif = this.handleChangeNif.bind(this);
        this.handleChangeEmail = this.handleChangeEmail.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangePasswordConfirm = this.handleChangePasswordConfirm.bind(this);
    }

    handleChangeName(event) {
        this.setState({nome: event.target.value});
    }

    handleChangeNif(event) {
        this.setState({nif: event.target.value});
    }

    handleChangeEmail(event) {
        this.setState({email: event.target.value});
    }

    handleChangePassword(event) {
        this.setState({password: event.target.value});
    }

    handleChangePasswordConfirm(event) {
        this.setState({passwordconfirm: event.target.value});
    }

    render() {
        return (
            <div className="container login-container">
                <div className="row">
                    <div className="col"></div>
                    <div className="col-md-7">
                        <div className="card login-card">
                            <div className="card-block">
                                <h4 className="card-title login-title">Gestão de Florestas</h4>
                                <p className="card-text login-text">Registar novo utilizador</p>
                                <form className="login-form">
                                    <div className="form-group">
                                        <input type="text" value={this.state.nome} className="form-control" id="textInput" onChange={this.handleChangeName} placeholder="Nome"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="text" value={this.state.nif} className="form-control" id="nifInput" onChange={this.handleChangeNif} placeholder="NIF"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="email" value={this.state.email} className="form-control" id="emailInput" onChange={this.handleChangeEmail} placeholder="Email"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="password" value={this.state.password} className="form-control" id="passwordInput" onChange={this.handleChangePassword} placeholder="Password"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="password" value={this.state.passwordconfirm} className="form-control" id="passwordConfirmInput" onChange={this.handleChangePasswordConfirm} placeholder="Confirmar Password"></input>
                                    </div>
                                    <div className="form-group">
                                        <label>
                                            Tipo de conta:<br></br>  
                                            <select value={this.state.value} onChange={this.handleChangeDropdown}>
                                                <option value="proprietario">Proprietário</option>
                                                <option value="inspetor">Inspetor</option>
                                                <option value="supervisor">Supervisor</option>
                                                <option value="funcionario">Funcionário da Câmara</option>
                                            </select>
                                        </label>
                                    </div>
                                    <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleSubmit} value="Registar" />
                                    <input className="btn login-btn btn-success btn-sm" type='button' value="Voltar" />

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

export default Login;
