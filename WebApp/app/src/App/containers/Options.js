import React from 'react';
import axios from 'axios';
import './Options.css';

class Options extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            newname: '',
            name: '',
            email: '',
            newemail: '',
            password: '',
            newpass: '',
            passconfirm: '',
            optionselect: 0,
            warnPassInequal: false,
            warnPassWrong: false,
            confirm: false
        };

        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleEmailChange = this.handleEmailChange.bind(this);
        this.handleCurrPass = this.handleCurrPass.bind(this);
        this.handleNewPass = this.handleNewPass.bind(this);
        this.handleConfPass = this.handleConfPass.bind(this);
        this.handleNameButton = this.handleNameButton.bind(this);
        this.handleEmailButton = this.handleEmailButton.bind(this);
        this.handlePasswordButton = this.handlePasswordButton.bind(this);
        this.optionDisplay = this.optionDisplay.bind(this);
        this.changeOption = this.changeOption.bind(this);
    }

    componentDidMount() {
        this.setState({ name: this.props.user.nome });
        if (this.props.accounttype === 'proprietarios') { this.setState({ email: this.props.user.mail }) } else {
            this.setState({ email: this.props.user.email })
        };
    }

    handleNameChange(event) {
        this.setState({ newname: event.target.value });
    }

    handleCurrPass(event) {
        this.setState({ password: event.target.value });
    }

    handleNewPass(event) {
        this.setState({ newpass: event.target.value });
    }

    handleConfPass(event) {
        this.setState({ passconfirm: event.target.value });
    }

    handleEmailChange(event) {
        this.setState({ newemail: event.target.value });
    }

    changeOption(option) {
        this.setState({ newemail: '', newname: '', password: '', newpass: '', passconfirm: '', warnPassWrong: false, warnPassInequal: false, confirm: false });
        this.setState({ optionselect: option });
    }

    optionDisplay() {
        switch (this.state.optionselect) {
            case 0:
                return null;
            case 1: //Name
                return (
                    <div className="form-group">
                        <p style={{ textAlign: 'left' }}>{'Novo Nome: '}</p>
                        <input type="text" className="form-control" value={this.state.newname} id="newName" onChange={this.handleNameChange} placeholder="Nome"></input>
                        <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleNameButton} value="Alterar" />
                        <input className="btn login-btn btn-success btn-sm" type='button' onClick={() => this.changeOption(0)} value="Cancelar" />
                    </div>
                );
            case 2: //E-mail
                return (
                    <div className="form-group">
                        <p style={{ textAlign: 'left' }}>{'Novo E-mail: '}</p>
                        <input type="text" className="form-control" value={this.state.newemail} id="newEmail" onChange={this.handleEmailChange} placeholder="E-mail"></input>
                        <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleEmailButton} value="Alterar" />
                        <input className="btn login-btn btn-success btn-sm" type='button' onClick={() => this.changeOption(0)} value="Cancelar" />
                    </div>
                );

            case 3: //Password
                return (
                    <div className="form-group">
                        <p style={{ textAlign: 'left' }}>{'Alterar Password: '}</p>
                        <input type="password" className="form-control" value={this.state.password} id="currPass" onChange={this.handleCurrPass} placeholder="Password Atual"></input>
                        <input type="password" className="form-control" value={this.state.newPassword} id="newPass" onChange={this.handleNewPass} placeholder="Nova Password"></input>
                        <input type="password" className="form-control" value={this.state.confirmPassword} id="cnfPass" onChange={this.handleConfPass} placeholder="Confirmar Password"></input>
                        <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handlePasswordButton} value="Alterar" />
                        <input className="btn login-btn btn-success btn-sm" type='button' onClick={() => this.changeOption(0)} value="Cancelar" />
                    </div>
                );
            default:
                return null;
        }
    }

    async handlePasswordButton(event) {
        this.setState({warnPassInequal: false, warnPassWrong: false, confirm: false});
        if (this.state.newpass === this.state.passconfirm) {
            await axios({
                method: 'put',
                url: 'https://localhost:44301/' + this.props.accounttype + '/info/changes/password',
                data: JSON.stringify(this.props.username + ',' + this.state.password + ',' + this.state.newpass),
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": this.state.auth
                }
            })
                .then(response => {
                    this.setState({ confirm: true });
                    console.log(this.response);
                    this.setState({ optionselect: 0 });
                })
                .catch(response => {
                    //alert("Erro na alteração de nome.");
                    this.setState({warnPassWrong: true});
                    console.log(this.response);
                })
        } else {
            this.setState({warnPassInequal: true});
        }
        //event.preventDefault();
    }

    async handleNameButton(event) {
        await axios({
            method: 'put',
            url: 'https://localhost:44301/' + this.props.accounttype + '/info/changes/nome',
            data: JSON.stringify(this.props.username + ',' + this.state.newname),
            headers: {
                "Content-Type": "application/json",
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({ confirm: true });
                this.setState({ name: this.state.newname });
                var newuser = this.props.user;
                newuser.nome = this.state.newname;
                this.props.change.user(newuser);
                console.log(this.response);
            })
            .catch(response => {
                //alert("Erro na alteração de nome.");
                console.log(this.response);
            })
        //event.preventDefault();
        this.setState({ optionselect: 0 });
    }


    async handleEmailButton(event) {
        await axios({
            method: 'put',
            url: 'https://localhost:44301/' + this.props.accounttype + '/info/changes/email',
            data: JSON.stringify(this.props.username + ',' + this.state.newemail),
            headers: {
                "Content-Type": "application/json",
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({ confirm: true });
                this.setState({ email: this.state.newemail });
                var newuser = this.props.user;
                if (this.props.accounttype === 'proprietarios') { newuser.mail = this.state.newemail }
                else { newuser.email = this.state.newemail };
                this.props.change.user(newuser);
            })
            .catch(response => {
                //alert("Erro na alteração de nome.");
                console.log(this.response);
            })
        this.setState({ optionselect: 0 });
        //event.preventDefault();
    }

    render() {
        return (
            <div className="container login-container">
                <div className="row">
                    <div className="col"></div>
                    <div className="col-md-10">
                        <div className="card login-card">
                            <div className="card-block">
                                <h4 className="card-title login-title options-header">Opções de Conta</h4>
                                <div className="optdiv d-flex w-100 justify-content-between">
                                    <p className="options-text">Nome: {this.state.name}</p>
                                    <svg className="bi bi-pencil-square pencil" onClick={() => this.changeOption(1)} width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                                        <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                                    </svg>
                                </div>
                                <div className="optdiv">
                                    <p className="options-text">E-mail: {this.state.email}</p>
                                    <svg className="bi bi-pencil-square pencil" onClick={() => this.changeOption(2)} width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                                        <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                                    </svg>
                                </div>
                                <p className="passedit" onClick={() => this.changeOption(3)}>Alterar password...</p>
                                {this.optionDisplay()}
                                <p>{this.state.confirm ? "Alteração efetuada com sucesso!" : null}</p>
                                <p>{this.state.warnPassInequal ? "Nova password e confirmação de password diferentes." : null}</p>
                                <p>{this.state.warnPassWrong ? "Password atual incorreta." : null}</p>
                            </div>
                        </div>
                    </div>
                    <div className="col" />
                </div>
            </div>
        )
    }
}

export default Options;