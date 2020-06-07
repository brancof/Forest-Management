import React from 'react';
import axios from 'axios';
import DirectionsMap from './DirectionsMap';
import {
    Link
  } from "react-router-dom";
import './Inspetores.css'

  class Inspetores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            percurso: []
        };
        this.sugestaoPercurso = this.sugestaoPercurso.bind(this);
    
    }

    componentDidMount()
    {
        this.sugestaoPercurso();
        
    }

    sugestaoPercurso() 
    {
        axios.get('https://localhost:44301/inspetores/Sugestaoinspecao', {
                params: {
                    Username: this.props.user.Username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({percurso: response.data});
                console.log(response.data);
            }) 
            .catch(response => {
                console.log(response);
            })
    }

  
    render() {
        return (
            <div className="container login-container">
                <div className="row">
                        <div className="col"></div>
                        <div className="col-md-10">
                            <div className="card login-card">
                                <div className="card-block">
                                    <h4 className="card-title login-title">Nome Inspetor</h4>
                                    <p className="card-text login-text">Propriedades para Inspeção</p>
                                    <table className="table table-hover table-bordered table-prop">
                                        <thead>
                                            <tr>
                                                <th scole="col">Morada</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                               <td className="linktext">Morada para Inspeção, nº 5, Braga</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                        <form>
                                            <input className="searchbox" type="text" placeholder="Procurar propriedade..." name="procurar" />
                                            <svg className="bi bi-search" width="1.2em" height="1.2em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                                <path fillRule="evenodd" d="M10.442 10.442a1 1 0 011.415 0l3.85 3.85a1 1 0 01-1.414 1.415l-3.85-3.85a1 1 0 010-1.415z" clipRule="evenodd"/>
                                                <path fillRule="evenodd" d="M6.5 12a5.5 5.5 0 100-11 5.5 5.5 0 000 11zM13 6.5a6.5 6.5 0 11-13 0 6.5 6.5 0 0113 0z" clipRule="evenodd"/>
                                            </svg>
                                        </form>
                                        <p className="linktext">Opções</p>
                                        <div className="map-container">
                                            <DirectionsMap Data = {this.state.percurso}/>
                                        </div>
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }
}

export default Inspetores;

