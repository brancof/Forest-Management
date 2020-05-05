import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";
import './Proprietarios.css'

class Proprietarios extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            terrenos: '',
            checked: []
        };

        this.loadTerrenos = this.loadTerrenos.bind(this);
        this.terrenostable = this.terrenostable.bind(this);
        this.handleChangeTerreno = this.handleChangeTerreno.bind(this);
        this.handleCheck = this.handleCheck.bind(this);

        this.loadTerrenos();
    }

    handleCheck(event)
    {
        this.state.checked[event.target.value] = event.target.checked;
        //this.forceUpdate();
    }

    terrenostable(){
        return (this.state.terrenos.length > 0 ? this.state.terrenos.map((terreno, index) =>
            <tr key={terreno.id_Terreno}>
               <td><input type="checkbox" disabled={terreno.estadoLimpeza} key={terreno.id_Terreno} onChange={this.handleCheck} value={index} className="form-check-input" id="checkmark"/>
               {terreno.morada} - {terreno.cod_postal}</td>
               <td>{terreno.estadoLimpeza? "Limpo" : "Não Limpo"}</td> 
            </tr>
            ) : null)
    }


    loadTerrenos() 
    {
        axios.get('https://localhost:44301/proprietarios/terrenos', {
                params: {
                    Username: this.props.username,
                    Password: this.props.password
                }
            })
            .then(response => {
                //alert("Login efectuado com successo.");
                this.setState({terrenos: response.data});
                console.log(response.data);
                response.data.map((terreno, index) => {
                    this.state.checked[index]=false;
                }
                );
                //this.forceUpdate();
            }) 
            .catch(response => {
                alert("Erro no carregamento de terrenos.");
                console.log(response);
            })
    }

    handleChangeTerreno(event)
    {
        event.preventDefault();
        
        if(!this.state.terrenos.length > 0) return;
        var i;
        for(i = 0; i < this.state.checked.length; i++)
        {
            if(this.state.checked[i])
            {
                axios({
                    method: 'put',
                    url: 'https://localhost:44301/proprietarios/limpeza',
                    data: JSON.stringify(this.props.username + ',' + this.props.password + ',' + this.state.terrenos[i].id_Terreno), 
                    headers: {
                        "Content-Type": "application/json"
                    }
                })
                    .then(response => {
                        console.log(response);
                        this.loadTerrenos();
                        //this.forceUpdate();
                    }) 
                    .catch(response => {
                        alert("Erro na limpeza do terreno.");
                        console.log(response);
                    })
            }
        }


        //alert(this.state.checked);
    }
    
    render() {
        
        return (
            <div className="container login-container">
                <div className="row">
                        <div className="col"></div>
                        <div className="col-md-10">
                            <div className="card login-card">
                                <div className="card-block">
                                    <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                    <p className="card-text login-text">Gestão de Propriedades</p>
                                    <table className="table table-hover table-bordered table-prop">
                                        <thead>
                                            <tr>
                                                <th scope="col">Morada</th>
                                                <th scope="col">Estado</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.terrenostable()}
                                        </tbody>
                                    </table>
                                    <input className="btn btn-success btn-sm btn-add-prop" type='submit' onClick={this.handleChangeTerreno} value="Alterar Estado" />
                                    <p className="linktext">Opções</p>
                                    <p className="linktext">Logout</p>
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }
}

export default Proprietarios;