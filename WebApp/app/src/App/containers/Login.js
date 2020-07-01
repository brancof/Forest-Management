import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";
import './Login.css';

class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: '',
            warning: '',
            wrongwarning: '',
            accounttype: 'proprietarios'
        };

        this.handleLoginButton = this.handleLoginButton.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangeUsername = this.handleChangeUsername.bind(this);
        this.handleChangeDropdown = this.handleChangeDropdown.bind(this);
        this.validateLogin = this.validateLogin.bind(this);
    }

    componentDidMount() {
    }

    componentWillUnmount() {
        this.props.pwinfo.set(false);
    }

    handleChangePassword(event) {
        this.setState({password: event.target.value});
    }

    handleChangeUsername(event) {
        this.setState({username: event.target.value});
    }

    handleChangeDropdown(event) {
        this.setState({accounttype: event.target.value});
    }

    validateLogin(event) {
        return this.state.username.length > 0 && this.state.password.length > 0;
    }

    handleLoginButton(event) {
        if(this.validateLogin())
        {
            this.setState({warning: false});
            this.setState({wrongwarning: false});
            axios.get('https://localhost:44301/' + this.state.accounttype + '/login', {
                params: {
                    Username: this.state.username,
                    Password: this.state.password
                }
            })
            .then(response => {
                this.props.change.token(response.data[1]);
                this.props.change.accounttype(this.state.accounttype);
                this.props.change.user(response.data[0]);
                this.props.change.username(this.state.username);
            }) 
            .catch(response => {
                this.setState({wrongwarning: true});
                console.log(response);
            })
        } else {
            this.setState({warning: true});
        }

        event.preventDefault();
    }

    render() {
        return (
            <div className="container login-container">
                <div className="row">
                    <div className="col"></div>
                    <div className="col-md-7">
                        <div className="card loginOnly-card">
                            <div className="card-block" >
                                <h3 style={{marginBottom: '20px'}} className="card-title login-title">Gestão de Florestas</h3>
                                <p style={{marginBottom: '40px'}} class="text-secondary">Website dedicado à gestão da limpeza 
                                e organização de florestas e territórios.</p>
                                <form className="login-form">
                                    <div className="form-group">
                                        <input type="text" value={this.state.username} className="form-control" id="usernameInput" onChange={this.handleChangeUsername} placeholder="Username"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="password" value={this.state.password} className="form-control" id="passwordInput" onChange={this.handleChangePassword} placeholder="Password"></input>
                                    </div>
                                    <div className="form-group">
                                        <label>
                                            Tipo de conta:<br></br>  
                                            <select value={this.state.accounttype} onChange={this.handleChangeDropdown}>
                                                <option value="proprietarios">Proprietário</option>
                                                <option value="inspetores">Inspetor</option>
                                                <option value="supervisores">Supervisor</option>
                                                <option value="trabalhadores">Funcionário da Câmara</option>
                                            </select>
                                        </label>
                                    </div>
                                    <div className="form-group">
                                        <p>{this.state.warning ? 'Campo username/password vazio.' : ''}</p>
                                        <p>{this.state.wrongwarning ? 'Username/password incorretos.' : ''}</p>
                                        <p>{this.props.pwinfo.show ? 'Password alterada com sucesso!' : ''}</p>
                                    </div>
                                    <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleLoginButton} value="Login" />
                                    <Link to="/registo"><input className="btn login-btn btn-success btn-sm" type='button' value="Registar" /></Link>
                                </form>
                                <Link to="/recuperar" className="forgotlink">Esqueci-me da password?</Link>
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