import React from 'react';
import axios from 'axios';
import DirectionsMap from './DirectionsMap';
import './Inspetores.css'


  class InspetoresInspecionar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            terrenos : this.props.terrenos,
            checked: [],
            displayTable: 0,
            textValue:'',
            resultado: ''
        };
        
        this.handleCheck = this.handleCheck.bind(this);
        this.terrenostable = this.terrenostable.bind(this);
        this.sugestaoPercurso = this.sugestaoPercurso.bind(this);
        this.handleChangeTextArea = this.handleChangeTextArea.bind(this);
        this.handleChangeResultado = this.handleChangeResultado.bind(this);
        this.handleCheckedProp = this.handleCheckedProp.bind(this);
        
    }

    componentDidMount()
    {
        if(this.state.terrenos.length > 0) this.setState({ displayTable: 1})
        if(this.state.terrenos.length === 0) this.sugestaoPercurso();
        
    }

    async sugestaoPercurso() 
    {
        await axios.get('https://localhost:44301/inspetores/Sugestaoinspecao', {
                params: {
                    Username: this.props.user.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({ terrenos: response.data, displayTable: 1});
                console.log(response.data);
            }) 
            .catch(response => {
                console.log(response);
            })
    }

    handleAlterar(event) {
        event.preventDefault();

        if (!this.state.terrenos.length > 0) return;
        if(this.props.idSelected != ""){
            axios({
                method: 'put',
                url: 'https://localhost:44301/inspetores/Realizarinspecao',
                data: JSON.stringify(this.props.user.username + '|' + this.state.resultado + '|' + this.state.textValue +'|'+ this.props.idSelected), 
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                console.log(response);
            }) 
            .catch(response => {
                alert("Erro na realizaçao da limpeza.");
                console.log(response);
            })
        }
        else{
            var i;
            for (i = 0; i < this.state.checked.length; i++) {
                if (this.state.checked[i]) {
                    axios({
                        method: 'put',
                        url: 'https://localhost:44301/inspetores/Realizarinspecao',
                        data: JSON.stringify(this.props.user.username + '|' + this.state.resultado + '|' + this.state.textValue +'|'+ this.state.terrenos[i].id_Terreno), 
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": this.state.auth
                        }
                    })
                    .then(response => {
                        console.log(response);
                    }) 
                    .catch(response => {
                        alert("Erro na realizaçao da limpeza.");
                        console.log(response);
                    })
                }
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
                        <input style={{display: "inline"}} type="checkbox" defaultChecked={terreno.id_Terreno==this.props.idSelected} disabled={terreno.estadoLimpeza} key={terreno.id_Terreno} onChange={this.handleCheck} value={index} className="form-check-input" id="checkmark"/>
                   </div>
               </td>
               <td className="colexpand" style={{textAlign: "left"}}>
                    <p style={{display: "inline"}}>{terreno.morada} - {terreno.cod_postal}</p> 
               </td>
            </tr>
            ) :this.setState({displayTable: 0}))
    }


    handleChangeTextArea(event) {
        this.setState({textValue: event.target.value});
    }

    handleChangeResultado(event) {
        this.setState({resultado: event.target.value});
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
                                    <p className="card-text login-text">Propriedades a Inspecionar</p>
                                    {this.state.displayTable === 0? "Não existem inspeções pendentes de momento.":
                                    (this.props.idSelected != ""? <p>{this.props.idSelected}</p>:
                                        <table className="table table-responsive table-hover table-bordered table-prop">
                                            <thead>
                                                <tr>
                                                    <th scole="col colexpand"></th>
                                                    <th scole="col colexpand">Morada</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {this.state.displayTable ? this.terrenostable() : null}
                                            </tbody>
                                        </table>
                                    )}
                                    <form>
                                    <div style={{marginTop: "5%"}}>
                                        <p>Resultado</p>
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="resultado" id="1" value="1" onChange={this.handleChangeResultado}/>
                                            <label class="form-check-label" for="exampleRadios1">
                                                Aprovado
                                            </label>
                                        </div> 
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="resultado" id="0" value="0" onChange={this.handleChangeResultado}/>
                                            <label class="form-check-label" for="exampleRadios1">
                                                Não Aprovado
                                            </label>
                                        </div>   
                                        <div style={{marginTop: "8%"}} class="form-group">
                                            <label for="exampleFormControlTextarea1">Relatório</label>
                                            <textarea class="form-control" id="exampleFormControlTextarea1" rows="7" value={this.state.textValue} onChange={this.handleChangeTextArea}/>
                                        </div>
                                    </div>  
                                    <div style={{marginTop: "5%"}}>
                                        <input className="btn login-btn btn-success btn-sm" type='submit' value="Enviar" on onClick={this.handleAlterar}/>
                                    </div> 
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

export default InspetoresInspecionar;

