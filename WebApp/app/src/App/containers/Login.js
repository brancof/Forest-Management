import React from 'react';
import axios from 'axios';
import './Login.css'

class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            poke: '' //temptest
        };

        this.handleLoginButton = this.handleLoginButton.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangeEmail = this.handleChangeEmail.bind(this);
    }

    componentDidMount() {

    }

    handleChangePassword(event) {
        this.setState({password: event.target.value});
    }

    handleChangeEmail(event) {
        this.setState({email: event.target.value});
    }

    handleLoginButton(event) {
        //alert(this.state.email);
                /*axios.get('https://localhost:5001/Login', {
            params: {
                Username: this.state.email,
                Password: this.state.password
            }
        }) Access-Control-Allow-Origin requests on server*/
        axios.get('https://pokeapi.co/api/v2/pokemon/'+this.state.email)
            .then(res => {
                const poke = res.data;
                this.setState({poke});        
                alert(this.state.poke.name);
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
                                        <input type="email" value={this.state.email} className="form-control" id="emailInput" onChange={this.handleChangeEmail} placeholder="Email"></input>
                                    </div>
                                    <div className="form-group">
                                        <input type="password" value={this.state.password} className="form-control" id="passwordInput" onChange={this.handleChangePassword} placeholder="Password"></input>
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