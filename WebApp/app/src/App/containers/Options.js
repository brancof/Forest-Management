import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";

  class Options extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            newname: '',
            name: '',
            confirm: false
        };

        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleNameButton = this.handleNameButton.bind(this);
    }

    componentDidMount(){
        this.setState({name: this.props.user.nome});
    }

    handleNameChange(event) {
        this.setState({newname: event.target.value});
    }


    async handleNameButton(event) {

        await axios({
            method: 'put',
            url: 'https://localhost:44301/proprietarios/info/changes/nome',
            data: JSON.stringify(this.props.username + ',' + this.state.newname), 
            headers: {
                "Content-Type": "application/json",
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({confirm: true});
                this.setState({name: this.state.newname});
                var newuser = this.props.user;
                newuser.nome = this.state.newname;
                this.props.change.user(newuser);
                console.log(this.response);
            }) 
            .catch(response => {
                //alert("Erro na alteração de nome.");
                console.log(this.response);
            })

        this.setState({newname: ''});
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
                            <h4 className="card-title login-title">{this.state.name}</h4>
                            <p className="card-text login-text">Alterar Nome</p>
                                <div className="form-group">
                                    <p style={{textAlign: 'left'}}>{'Novo Nome: '}</p>
                                    <input type="text" className="form-control" value={this.state.newname} id="newName" onChange={this.handleNameChange} placeholder="Nome"></input>
                                    <input className="btn login-btn btn-success btn-sm" type='submit' onClick={this.handleNameButton} value="Alterar" />
                                </div>
                                <p>{this.state.confirm ? "Nome alterado com sucesso." : null}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default Options;