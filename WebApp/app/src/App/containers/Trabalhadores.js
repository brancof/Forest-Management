import React from 'react';
import axios from 'axios';
import DirectionsMap from './DirectionsMap';
import './Trabalhadores.css'

  class Trabalhadores extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            terrenos: '',
            checked: [],
            displayTable: 0
        };

        this.terrenosPendentes = this.terrenosPendentes.bind(this);
        this.terrenostable = this.terrenostable.bind(this);
        
        
    }

    componentDidMount()
    {
        this.terrenosPendentes();
    }

    
    terrenosPendentes() {
        axios.get('https://localhost:44301/trabalhadores/LimpezasPendentes', {
            params: {
                Username: this.props.username
            },
            headers: {
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({ terrenos: response.data , displayTable : 1});
                console.log(response.data);
            })
            .catch(response => {
                alert("Erro no carregamento de terrenos pendentes.");
                console.log(response);
            })
    }

    terrenostable(){
        return (this.state.terrenos.length > 0 ? this.state.terrenos.map((terreno, index) =>
            <tr key={terreno.id_Terreno}>
               <td style={{textAlign: "left", paddingLeft: "1%"}}>
                    <div class="custom-control custom-checkbox">
                        <input style={{display: "inline"}} type="checkbox"  disabled={terreno.estadoLimpeza} key={terreno.id_Terreno} onChange={this.handleCheck} value={index} className="form-check-input" id="checkmark"/>
                   </div>
               </td>
               <td className="colexpand" style={{textAlign: "left"}}>
                    <p style={{display: "inline"}}>{terreno.morada} - {terreno.cod_postal}</p> 
               </td>
               <td style={{textAlign: "left"}}>{terreno.estadoLimpeza? 'Realizada' : 'Pendente'}</td> 
            </tr>
            ) : "Nenhum terreno encontrado.")
    }
    
  
    render() {
        return (
            <div className="container login-container">
                <div className="row">
                        <div className="col"></div>
                        <div className="col-md-10">
                            <div className="card login-card">
                                <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                <p className="card-text login-text">Gestão de Trabalho</p>
                                <h5 style={{ textAlign: 'left' }} className="card-title login-title">{this.props.user.concelho}</h5>
                                <div className="div-space">
                                <table className="table table-hover">
                                    <thead>
                                        <tr class="table-active">
                                            <th scope="col"></th>
                                            <th scope="col">Morada</th>
                                            <th scope="col colexpand">Estado</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {this.state.displayTable ? this.terrenostable() : null}
                                    </tbody>
                                </table>
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }
}
export default Trabalhadores;