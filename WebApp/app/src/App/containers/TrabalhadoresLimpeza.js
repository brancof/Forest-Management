import React from 'react';
import axios from 'axios';
import './Trabalhadores.css'

class TrabalhadoresLimpeza extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            terrenos: '',
            checked: [],
            displayTable: 0,
            sucesso: 0
        };

        this.terrenosPendentes = this.terrenosPendentes.bind(this);
        this.terrenostable = this.terrenostable.bind(this);
        this.handleAlterar = this.handleAlterar.bind(this);
        this.handleCheck = this.handleCheck.bind(this);
        this.handleCheckedProp = this.handleCheckedProp.bind(this);
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
                //console.log(response.data);
            })
            .catch(response => {
                alert("Erro no carregamento de terrenos pendentes.");
                console.log(response);
            })
    }


    handleAlterar(event) {
        event.preventDefault();

        if (!this.state.terrenos.length > 0) return;
        var i;
        for (i = 0; i < this.state.checked.length; i++) {
            if (this.state.checked[i]) {
                axios({
                    method: 'put',
                    url: 'https://localhost:44301/trabalhadores/Limpeza',
                    data: JSON.stringify(this.props.user.username + ',' + this.state.terrenos[i].id_Terreno), 
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": this.state.auth
                    }
                })
                .then(response => {
                    this.setState({sucesso: 1});
                    this.terrenosPendentes();
                    var v;
                    for (v = 0; v < this.state.checked.length; v++) this.state.checked[v] = 0;
                    //console.log(response);
                }) 
                .catch(response => {
                    alert("Erro na realizaçao da limpeza.");
                    console.log(response);
                })
            }
        }
    }

    handleCheck(event) {
        this.state.checked[event.target.value] = event.target.checked;
    }

    handleCheckedProp(index) {
        this.state.checked[index] = 1;
    }

    terrenostable(){
        return (this.state.terrenos.length > 0 ? this.state.terrenos.map((terreno, index) =>
            <tr key={terreno.id_Terreno}>
               <td style={{textAlign: "left", paddingLeft: "1%"}}>
                    <div class="custom-control custom-checkbox">
                        {terreno.id_Terreno==this.props.idSelected? this.handleCheckedProp(index): null}
                        <input style={{display: "inline"}} type="checkbox" defaultChecked={terreno.id_Terreno==this.props.idSelected}  disabled={terreno.estadoLimpeza} key={terreno.id_Terreno} onChange={this.handleCheck} value={index} className="form-check-input" id="checkmark"/>
                   </div>
               </td>
               <td className="colexpand" style={{textAlign: "left"}}>
                    <p style={{display: "inline"}}>{terreno.morada} - {terreno.cod_postal}</p> 
               </td>
               <td style={{textAlign: "left"}}>{terreno.estadoLimpeza? 'Realizada' : 'Pendente'}</td> 
            </tr>
            ) :this.setState({displayTable: 0}))
    }

  
    render() {
        return (
            <div className="container login-container">
                <div className="row">
                        <div className="col"></div>
                        <div className="col-md-10">
                            <div className="card login-card">
                                    <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                    <div className="card-block">
                                    <p className="card-text login-text">Gestão de Trabalho</p>
                                    <h5 style={{ textAlign: 'left' }} className="card-title login-title">{this.props.user.concelho}</h5>
                                    <div className="div-space">
                                        {this.state.displayTable ===0? "Não existem limpezas pendentes de momento.":
                                            <table className="table table-hover table-responsive table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th scope="col"></th>
                                                        <th scope="col">Morada</th>
                                                        <th scope="col colexpand">Estado</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    {this.state.displayTable ? this.terrenostable() : null}
                                                </tbody>
                                            </table>
                                        }
                                    <div>
                                        {this.state.sucesso === 1? <p>O terreno foi limpo com sucesso</p> : null}
                                    </div>
                                    <div>
                                        {this.state.displayTable ===0? null: <input className="btn btn-success btn-sm btn-add-prop" type='submit' onClick={this.handleAlterar} value="Alterar Estado" />}
                                    </div>
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
export default TrabalhadoresLimpeza;