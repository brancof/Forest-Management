import React from 'react';
import axios from 'axios';
import './Supervisores.css'

class SupervisoresMarcarInsp extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            zonas: '',
            auth: "Bearer " + this.props.token,
            checked: [],
            displayTable: 0,
            inspClicked: 0,
            numberChecked: 0

        };

        this.zonasConcelho = this.zonasConcelho.bind(this);
        this.zonastable = this.zonastable.bind(this);
        this.handleMarcarInspecaoButton = this.handleMarcarInspecaoButton.bind(this);
        this.handleCheck = this.handleCheck.bind(this);
    
    }

    componentDidMount()
    {
        this.zonasConcelho();
    }

    handleCheck(event)
    {
        this.state.checked[event.target.value] = event.target.checked;
    }


    zonasConcelho() 
    {
        axios.get('https://localhost:44301/supervisores/Zonasconcelho', {
                params: {
                    Username: this.props.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({zonas: response.data});
                console.log(response.data);
            }) 
            .catch(response => {
                console.log(response);
            })
    }

    handleMarcarInspecaoButton(event) {
        event.preventDefault();
        
        if(!this.state.zonas.length > 0) return;
        var i;
        for(i = 0; i < this.state.checked.length; i++)
        {
            if(this.state.checked[i] && !this.state.zonas[i].emInspecao)
            {
                axios({
                    method: 'post',
                    url: 'https://localhost:44301/supervisores/Agendarinspecao',
                    data: JSON.stringify(this.props.username + ',' + this.state.zonas[i].codigo_Postal),  
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": this.state.auth
                    }
                })
                .then(response => {
                    this.setState({inspClicked: 1});
                    var count = 0;
                    for(i = 0; i < this.state.checked.length; i++)
                    {
                        if(this.state.checked[i]){
                            count++;
                        }
                    }
                    this.setState({numberChecked: count});
                    this.zonasConcelho();
                    console.log(response.data);
                }) 
                .catch(response => {
                    alert("Erro na marcação de uma inspeção.");
                    console.log(response);
                })
            }
        }
    }

    zonastable(){
        return (this.state.zonas.length > 0 ? this.state.zonas.map((zona, index) =>
            <tr key={zona.codigo_Postal}>
               <td style={{textAlign: "left", paddingLeft: "2%"}}>
                    <div className="custom-control custom-checkbox checksuper">
                        <input style={{display: "inline", visibility: zona.emInspecao ? "hidden" : "visible"}} type="checkbox" key={zona.codigo_Postal} onChange={this.handleCheck} value={index} className="form-check-input" id="checkmark"/>
                    </div>
               </td>
               <td className="colexpand">
                    <p style={{display: "inline"}}>{zona.codigo_Postal} - {zona.nomeFreguesia}</p> 
               </td>
               <td>
                    <p style={{display: "inline"}}>{zona.nivelCritico}</p> 
                </td>
                <td>
                    <p style={{display: "inline"}}>{zona.emInspecao ? "Agendada" : ""}</p> 
                </td>
            </tr>
            ) : "Nenhuma zona encontrado.")
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
                                    <p className="card-text login-text">Gestão de Concelhos</p>
                                    <h5 style={{textAlign: 'left'}} className="card-title login-title">{this.props.user.concelho}</h5>
                                    <p  style={{textAlign: 'left'}} className="card-text login-text">{'Escolha as Zonas a inspecionar '}</p>
                                    <table className="table table-responsive table-hover table-bordered ">
                                        <thead>
                                            <tr>
                                                <th scope="col"></th>
                                                <th scope="col colexpand">Local</th>
                                                <th scope="col">Prioridade</th>
                                                <th scope="col">Estado</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.zonastable()}
                                        </tbody>
                                    </table>
                                    <p  style={{textAlign: 'left'}} className="card-text login-text">{this.state.inspClicked?'Inspeção marcada para '+ this.state.numberChecked + ' zonas.':''}</p>
                                    <input className="btn btn-success btn-sm btn-add-prop" type='submit' onClick={this.handleMarcarInspecaoButton} value="Inspecionar" />
                                </div>
                            </div>
                        </div>
                        <div className="col"></div>
                </div>
            </div>
        );
    }
}

export default SupervisoresMarcarInsp;