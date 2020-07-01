import React from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import './Inspetores.css'



  class InspetoresInspecionar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            textValue:'',
            resultado: '',
            sucesso: 0,
            selecionado: 0
        };
        this.handleAlterar = this.handleAlterar.bind(this);
        this.handleChangeTextArea = this.handleChangeTextArea.bind(this);
        this.handleChangeResultado = this.handleChangeResultado.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleErro = this.handleErro.bind(this);
        this.handleAlterar = this.handleAlterar.bind(this);
        
    }

    handleAlterar(event) {
        event.preventDefault();

        if (this.state.textValue === '' ||  this.state.resultado === '') return;
        axios({
            method: 'put',
            url: 'https://localhost:44301/inspetores/Realizarinspecao',
            data: JSON.stringify(this.props.user.username + '|' + this.state.resultado + '|' + this.state.textValue +'|'+ this.props.selected.id_Terreno), 
            headers: {
                "Content-Type": "application/json",
                "Authorization": this.state.auth
                }
            })
            .then(response => {
                console.log(response);
                this.setState({sucesso: 1});
            }) 
            .catch(response => {
                //alert("Erro na realizaçao da limpeza.");
                this.setState({sucesso: 2});
                console.log(response);
            }
        )
    }


    handleChangeTextArea(event) {
        const regex = /^[^|]*$/;
        if (event.target.value === '' || regex.test(event.target.value)) {
            this.setState({textValue: event.target.value});
        }
    }

    handleChangeResultado(event) {
        this.setState({resultado: event.target.value});
    }


    handleSubmit(){
        return(
            <div>
                <div style={{textAlign:"Left"}}>
                    <h5>Terreno Selecionado</h5>
                    {this.props.selected != null?
                        <p>{this.props.selected.morada} - {this.props.selected.cod_postal}</p>
                        :   <p>Nenhum</p>
                    }
                    <form>
                        <div style={{marginTop: "10%"}}>
                            <h6>Resultado</h6>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="resultado" id="1" value='1' onChange={this.handleChangeResultado}/>
                                <label class="form-check-label" for="exampleRadios1">
                                    Aprovado
                                </label>
                            </div> 
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="resultado" id="0" value='0' onChange={this.handleChangeResultado}/>
                                <label class="form-check-label" for="exampleRadios1">
                                    Não Aprovado
                                </label>
                            </div>   
                            <div style={{marginTop: "5%"}}>
                                <h6>Relatório</h6>
                                <small class="text-secondary">(Máx. 200 caracteres)</small>
                                <textarea maxLength="200" class="form-control" id="exampleFormControlTextarea1" rows="7" value={this.state.textValue} onChange={this.handleChangeTextArea}/>
                            </div>
                        </div>
                        {this.props.selected != null?
                            <div style={{marginTop: "5%", textAlign:"Right"}}>
                                <input  className="btn login-btn btn-success btn-sm" type='submit' value="Enviar" onClick={this.handleAlterar}/>
                            </div>
                        : null
                        }
                                    
                    </form>
                </div>
            </div>
        )

    }

    handleSucesso(){
        return(
            <div style={{marginTop: "10%"}}>
                <h5>Sucesso</h5>
                <p>O relatório foi submetido com sucesso</p>
                <div style={{marginTop: "5%", textAlign:"Left"}}>
                    <Link to='/' class="btn login-btn btn-success btn-sm">
                        Voltar
                    </Link>
                </div>
            </div>
        )

    }

    handleErro(){
        return(
            <div style={{marginTop: "10%"}}>
                <h5>Ups, ocorreu um erro!</h5>
                <p>O relatório não foi submetido</p>
                <div style={{marginTop: "5%", textAlign:"Left"}}>
                    <Link to='/' class="btn login-btn btn-success btn-sm">
                        Voltar
                    </Link>
                </div>
            </div>
        )
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
                                    {this.state.sucesso === 0? 
                                        this.handleSubmit()
                                    : this.handleSucesso()
                                    }
                                    {this.state.sucesso === 2?
                                        this.handleErro()
                                    : null
                                    }
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

