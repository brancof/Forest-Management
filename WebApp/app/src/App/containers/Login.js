import React from 'react';
import axios from 'axios';
import './Login.css'

class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: '',
            accounttype: 'proprietarios'
        };

        this.handleLoginButton = this.handleLoginButton.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangeUsername = this.handleChangeUsername.bind(this);
        this.handleChangeDropdown = this.handleChangeDropdown.bind(this);
    }

    componentDidMount() {

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

    handleLoginButton(event) {
        //alert(this.state.email);
            axios.get('https://localhost:44301/' + this.state.accounttype + '/login', {
            params: {
                Username: this.state.username,
                Password: this.state.password
            }
        })
        .then(response => {
            alert("Login efectuado com successo.");
            console.log(response);
        }) 
        .catch(response => {
            alert("Username/password incorretos.");
            console.log(response);
        })

        event.preventDefault();
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
                                <p className="card-text login-text">Website dedicado à gestão da limpeza 
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
                                    <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleLoginButton} value="Login" />
                                    <input className="btn login-btn btn-success btn-sm" type='button' value="Registar" />
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