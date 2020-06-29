import React from 'react';
import axios from 'axios';
import {
    Link,
    Switch,
    Route,
    withRouter
} from "react-router-dom";
import RecuperarTkn from './RecuperarTkn';
import './RecuperarPass.css';


class RecuperarPass extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            warning: false,
            accounttype: 'proprietarios',
        };
        this.handleChangeUsername = this.handleChangeUsername.bind(this);
        this.handleChangeDropdown = this.handleChangeDropdown.bind(this);
        this.handleRecuperarButton = this.handleRecuperarButton.bind(this);
    }

    handleChangeDropdown(event) {
        this.setState({ accounttype: event.target.value });
    }

    handleChangeUsername(event) {
        this.setState({ username: event.target.value });
    }

    handleRecuperarButton(event) {
        event.preventDefault();

        if (this.state.username.length === 0) {
            this.setState({ warning: true });
        } else {
            this.setState({warning: false});
            axios({
                method: 'put',
                url: 'https://localhost:44301/' + this.state.accounttype + '/Resetpassword',
                data: JSON.stringify(this.state.username),
                headers: {
                    "Content-Type": "application/json",
                }
           })
        .then(
            this.props.history.push("/recuperar/token")
        )
        }
    }

    render() {
        return (
            <Switch>
                <Route exact path="/recuperar">
                    <div className="container login-container">
                        <div className="row">
                            <div className="col"></div>
                            <div className="col-md-7">
                                <div className="card loginOnly-card">
                                    <div className="card-block" >
                                        <h3 style={{ marginBottom: '20px' }} className="card-title login-title">Gestão de Florestas</h3>
                                        <p style={{ marginBottom: '40px' }} class="text-secondary">Recuperação de Password</p>
                                        <form className="login-form">
                                            <div className="form-group">
                                                <input type="text" value={this.state.username} className="form-control" id="usernameInput" onChange={this.handleChangeUsername} placeholder="Username"></input>
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
                                                <p>{this.state.warning ? 'Campo username vazio.' : ''}</p>
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
                </Route>

                <Route path="/recuperar/token">
                    <RecuperarTkn username={this.state.username} accounttype={this.state.accounttype}/>
                </Route>
            </Switch>
        );
    }
}

export default withRouter(RecuperarPass);
