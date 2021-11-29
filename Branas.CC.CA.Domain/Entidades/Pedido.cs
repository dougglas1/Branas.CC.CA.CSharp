﻿using Branas.CC.CA.Domain.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Branas.CC.CA.Domain.Entidades
{
    public class Pedido
    {
        public decimal ValorSubTotal { get; private set; }
        public decimal ValorTotal { get; private set; }
        public IEnumerable<Item> Itens => _itens;
        public Cpf Cpf { get; private set; }
        public Cupom Cupom { get; private set; }
        public Frete Frete { get; private set; }
        public Status Status { get; private set; }

        private List<Item> _itens;

        public Pedido(Cpf cpf)
        {
            _itens = new List<Item>();
            Cpf = cpf;
            AdicionarStatus();
        }

        public Pedido AdicionarStatus()
        {
            Status = Cpf.Numero.ValidarCpf() ? Status.Realizado : Status.Cancelado;

            return this;
        }
        
        public Pedido AdicionarItens(IEnumerable<Item> itens)
        {
            _itens.AddRange(itens);
            CalcularValorSubTotal();

            return this;
        }

        public Pedido AdicionarCupom(Cupom cupom)
        {
            Cupom = cupom;
            CalcularValorTotal();

            return this;
        }
        
        public Pedido AdicionarFrete(Frete frete)
        {
            Frete = frete;
            CalcularValorTotal();

            return this;
        }
        
        private void CalcularValorSubTotal()
        {
            ValorSubTotal = _itens.Sum(item => item.Preco);
        }
        
        private void CalcularValorTotal()
        {
            var descontoCupom = Cupom != null ? (ValorSubTotal * (Cupom.Porcentagem / 100)) : 0;
            var descontoFrete = Frete != null ? Frete.Valor : 0;
            
            ValorTotal = ValorSubTotal - descontoCupom - descontoFrete;
        }
    }
}
